# HaileyRepo

MIM ECMA2 (Extensible Connectivity 2.2) connector for [HaileyHR](https://api.haileyhr.app/docs/index.html)
(Swagger spec at `/docs/v1/docs.json`).

## Config parameters (Connectivity page)

| Parameter | Description |
|---|---|
| `BaseUrl` | HaileyHR API base URL, e.g. `https://api.haileyhr.app`. |
| `AccessToken` | A HaileyHR personal access token (a JWT you create in the Hailey UI, under the user's API access settings). Sent as `Authorization: Bearer <token>`. There is no token exchange/refresh flow — the token itself is the long-lived credential, so rotate it in Hailey and update this parameter if it's ever revoked or expires. |

## Object types / schema

Single object type: **Person**, anchored on `EmployeeId`.

- Most attributes come straight from `GET /Employees` (`EmployeeListItem` — ~48 flat
  fields: name, dates, org unit ids, bank details, address, emergency contact, etc).
- For each employee, the connector also calls `GET /Employees/{employeeId}` and merges in:
  - The **effectual employment** (`Effectual*` attributes) — picked from
    `jobData.employment.employments[]` by preferring the `Active` record (lowest
    `Priority` wins ties), else the soonest `WillStart` record, else the most recently
    `Ended` record. Which record was picked is logged per employee.
  - The **current salary** (`CurrentSalary*` attributes) — the first salary with no
    `EndDate` (still ongoing), else the one with the latest `EndDate`. If more than one
    ongoing salary exists for an employee, a warning is logged and the first one returned
    by the API is used (ambiguous case — HaileyHR's API doesn't say which one is
    "current" in that situation).
- Company-defined custom fields (`customFieldsData` on `EmployeeListItem`) are dynamic
  per tenant, so they're not part of the reflected schema — instead, `MakeSchema` samples
  every employee returned by `GET /Employees` and adds one `CustomField_<key>` attribute
  per distinct key it observes (multi-valued if any sample value was a JSON array, e.g. a
  multi-select field).

Because `GET /Employees` has no date filter or pagination, **every import run fetches a
full snapshot** — `DeltaImport` is set to `false` accordingly. This affects run profile
design: there's no "delta import" step to configure for this MA, only full imports.

## Export

HaileyHR's write API is much narrower than its read API — there's no general
create-or-update-the-whole-record endpoint:

- **Add** → `POST /Employee` ("quick add"). Requires `FirstName`, `LastName`,
  `CompanyEmail` and a start date (mapped from the `DateOfJoining` attribute); optionally
  `DepartmentId`, `LegalEntityId`, `CostCenterId`, `BusinessAreaId`, `LocationId`,
  `ManagerEmployeeId` (→ HaileyHR's `reportsToEmployeeId`), `TeamIds`, `TitleIds`. The
  response has no body and no documented `Location` header, so after a successful create
  the connector re-fetches `GET /Employees` and looks the new employee up by
  `CompanyEmail` to learn its `employeeId` (returned to MIM as the anchor). If that lookup
  doesn't find a match, the entry is marked `ExportErrorCustomContinueRun` with a message
  telling you to check HaileyHR manually and re-run import.
- **Update** → only two fields are actually writable:
  - `CompanyEmail` → `PUT /Employee/{employeeId}/CompanyEmail`
  - Any `CustomField_*` attribute → merged into one
    `PUT /Employee/{employeeId}/CustomEmployeeField` call.
  Any other changed attribute is **silently ignored** (logged as a warning) — it's
  import-only. If you need more fields writable, check whether HaileyHR has added more
  targeted PUT endpoints since this connector was generated.
- **Delete** → not supported by HaileyHR's API at all; returns
  `ExportErrorCustomStopRun` with a message to offboard the employee manually in Hailey.
- **Rename** → not applicable (`ObjectRename = false`).

### Known limitations / things to verify against real data

- `customFieldsData` keys are assumed to be usable directly as part of a MIM attribute
  name (spaces replaced with underscores, same convention used elsewhere in this
  codebase). On export, the `CustomField_` prefix is stripped and underscores are turned
  back into spaces to reconstruct the original key — if a real custom field key contains
  underscores as a literal character (not from space-replacement), this round-trip will
  be wrong. Verify against a real sample before relying on custom field export.
- `Salary.currencyCode`'s Swagger schema is an empty object (`{"type":"object"}`) rather
  than a proper string enum — this connector treats it as a plain string
  (`CurrentSalaryCurrencyCode`). Confirm the real shape against a live response.
- Only the "effectual" employment and "current" salary are synced — full employment
  history, casual employments, salary history/pending-approval workflow detail, feedback,
  360 reviews and education/certification records are all out of scope (not identity
  data). Add more merged fields from `GET /Employees/{employeeId}` if you need them.
- The per-employee detail call means import does **N+1 HTTP requests** (one `GET
  /Employees` + one `GET /Employees/{id}` per employee). For large employee counts this
  will be slow and is worth watching for HaileyHR-side rate limiting; if it becomes a
  problem, the easiest fix is to drop the per-employee merge and rely on
  `EmployeeListItem`'s fields alone (already covers most of what `Employee` returns).

## Troubleshooting

- Log file: `%ProgramData%\HaileyRepo\HaileyRepo.log` on the MIM Sync Service server.
- Every HTTP call, the picked effectual employment/salary per employee, skipped duplicate
  anchors, and every export outcome are logged there — check it first.
- A `401`/`403` on any call almost always means the `AccessToken` has expired or been
  revoked in Hailey — generate a new personal access token.

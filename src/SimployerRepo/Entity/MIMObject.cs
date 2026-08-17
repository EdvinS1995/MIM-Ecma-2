using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimployerRepo.Entity
{
    public class MIMObject
    {
        // ── EKSISTERENDE FELTER (beholdt) ────────────────────────────────────
        public string company { get; set; }
        public string department { get; set; }
        public string employeeID { get; set; }
        public string employeeNumber { get; set; }
        public string employeetype { get; set; }
        public string givenName { get; set; }
        public string location { get; set; }
        public string manager { get; set; }
        public string mobile { get; set; }
        public string office { get; set; }
        public string postOfficeBox { get; set; }
        public string postalCode { get; set; }
        public string roomNumber { get; set; }
        public object state { get; set; }
        public string streetAddress { get; set; }
        public string surname { get; set; }
        public string telephone { get; set; }
        public string title { get; set; }
        public string countryCode { get; set; }
        public object preferredLanuage { get; set; }
        public string hireDate { get; set; }
        public string expireDate { get; set; }
        public string template { get; set; }

        // ── SIMPLOYER PERSON ─────────────────────────────────────────────────
        public string PersonId { get; set; }
        public string Fornavn { get; set; }
        public string Etternavn { get; set; }
        public string PersonAktiv { get; set; }
        public string Epost { get; set; }
        public string Telefon { get; set; }

        // ── SIMPLOYER LEDER ──────────────────────────────────────────────────
        public string LederNavn { get; set; }
        public string LederId { get; set; }

        // ── SIMPLOYER ORGANISASJON ───────────────────────────────────────────
        public string AvdelingNavn { get; set; }
        public string AvdelingId { get; set; }
        public string AvdelingKode { get; set; }
        public string AvdelingAktiv { get; set; }

        // ── SIMPLOYER EMPLOYMENT ─────────────────────────────────────────────
        public string Stilling { get; set; }
        public string Ansattnummer { get; set; }
        public string Ansattdato { get; set; }
        public string Sluttdato { get; set; }
        public string Stillingsprosent { get; set; }
        public string Kategori { get; set; }
        public string EmploymentId { get; set; }

        // ── SIMPLOYER EMPLOYEE ───────────────────────────────────────────────
        public string EmployeeId { get; set; }
        public string EmployeeAktiv { get; set; }

        // ── SIMPLOYER BRUKER ─────────────────────────────────────────────────
        public string BrukerEpost { get; set; }

        // ── MAPPING FRA SIMPLOYER TIL EKSISTERENDE FELTER ───────────────────
        /// <summary>
        /// Mapper Simployer-felter til eksisterende MIMObject-felter
        /// slik at resten av MIM-løsningen fungerer som før
        /// </summary>
        public void MapFromSimployer()
        {
            givenName = Fornavn;
            surname = Etternavn;
            employeeNumber = Ansattnummer;
            employeeID = Ansattnummer;
            title = Stilling;
            department = AvdelingNavn;
            manager = LederNavn;
            mobile = Telefon;
            hireDate = Ansattdato;
            expireDate = Sluttdato;
            employeetype = Kategori;
        }
    }
}
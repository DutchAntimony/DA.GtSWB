using DA.GtSWB.Application.Ledenadministratie.Adressen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.GtSWB.Tests.UnitTests.Application.Ledenadminstratie;
public static class ValidDtoProvider
{
    public static AdresDto AdresDto =>
        new()
        {
            Straat = "  Straat",
            Huisnummer = "1A ",
            Postcode = "1234  ab",
            Woonplaats = " Woonplaats ",
            Land = null
        };
}

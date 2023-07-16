﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TES3Lib.Base;

namespace Tes3EditX.Backend.Extensions;

public static class Tes3Extensions
{
    public static string GetUniqueId(this Record record)
    {
        return $"{record.Name},{record.GetEditorId()}";
    }

    public static ulong GetUniqueNameHash(this Record record)
    {
        return 0;
    }

    public static IEnumerable<string> GetAllTags()
    {
        return new List<string>()
    {
        "",
        "TES3",
        "GMST",
        "GLOB",
        "CLAS",
        "FACT",
        "RACE",
        "SOUN",
        "SNDG",
        "SKIL",
        "MGEF",
        "SCPT",
        "REGN",
        "BSGN",
        "SSCR",
        "LTEX",
        "SPEL",
        "STAT",
        "DOOR",
        "MISC",
        "WEAP",
        "CONT",
        "CREA",
        "BODY",
        "LIGH",
        "ENCH",
        "NPC_",
        "ARMO",
        "CLOT",
        "REPA",
        "ACTI",
        "APPA",
        "LOCK",
        "PROB",
        "INGR",
        "BOOK",
        "ALCH",
        "LEVI",
        "LEVC",
        "CELL",
        "LAND",
        "PGRD",
        "DIAL",
        "INFO",
    };
    }
}

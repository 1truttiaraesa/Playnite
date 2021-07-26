﻿using Playnite.SDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Playnite.SDK
{
    public interface IEmulationAPI
    {
        IList<EmulatedPlatform> Platforms { get; }
        EmulatedPlatform GetPlatform(string platformId);
    }
}

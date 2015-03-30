﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ian.Configuration
{
    public interface IConfigurationReader
    {
        object GetSection(string sectionName);

        T GetSection<T>(string sectionName) where T : class, new();
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DMS_M306.ViewModels.File
{
    public class FileDetailsViewModel : FileViewModel
    {
        public List<ReleaseViewModel> Releases { get; set; }
    }
}
﻿using System;
using Newtonsoft.Json;

namespace CompostOutputsFertilizer
{
    public class Config
    {
        [JsonProperty]
        public bool IgnoreIfOnlyDragging { get; set; } = false;
        [JsonProperty]
        public bool IgnoreToolQueue { get; set; } = false;
        [JsonProperty]
        public bool ResetToolOnAutosave { get; set; } = false;
        [JsonProperty]
        public bool SendAutosaveWarning { get; set; } = false;
        [JsonProperty]
        public int WarningSecondsBeforeAutosave { get; set; } = 10;
    }
}

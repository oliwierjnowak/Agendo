﻿namespace Agendo.Shared.Form.CreateShift
{
    public record CreateShift
    {
        public int Nr { get; set; }
        public string Name { get; set; }
        public int Hours { get; set; }
        public string Color { get; set; }
    }
}

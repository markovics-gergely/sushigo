﻿using Microsoft.AspNetCore.Http;
using shared.bll.Validators.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shared.bll.Validators.Implementations
{
    public class FileSizeValidator : IValidator
    {
        private readonly IFormFile formFile;
        private readonly int maxSize;

        public FileSizeValidator(IFormFile formFile, int maxSize)
        {
            this.formFile = formFile;
            this.maxSize = maxSize;
        }

        public bool Validate()
        {
            return formFile.Length > 0 && formFile.Length <= maxSize * Math.Pow(1024, 2);
        }
    }
}

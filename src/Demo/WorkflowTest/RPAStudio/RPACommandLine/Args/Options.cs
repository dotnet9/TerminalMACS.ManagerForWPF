using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPACommandLine.Args
{
    public class Options
    {
        [Option('f', "file", Required = true, HelpText = "The xaml file or project.rpa file.")]
        public string File { get; set; }

        [Option('i', "input", HelpText = "Input arguments in json string format.")]
        public string Input { get; set; }

        [Option('l', "log", HelpText = "Logs write to file.")]
        public string Log { get; set; }
    }
}
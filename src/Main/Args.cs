using CommandLineParser.Arguments;

namespace Main
{
    public class Args
    {
        public static DirectoryArgument output = new DirectoryArgument('o', "output", "Path to output files to");
        public static ValueArgument<string> values = new ValueArgument<string>('v', "values", "Values to download from, example: \"809943207680802848 & 207930882294546432\"");

    }
}
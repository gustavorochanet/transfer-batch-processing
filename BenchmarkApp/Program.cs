using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using MainApp.FileManager;

//var summary = BenchmarkRunner.Run<FileReaderBenchmark>();

var summary = BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, new DebugInProcessConfig());

//static void Main(string[] args) => BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, new DebugInProcessConfig());




public class FileReaderBenchmark
{
    private readonly FileReader _fileReaderOld;
    private readonly FileReader _fileReaderNew;

    public FileReaderBenchmark()
    {
        string filePath = "large_transactions.csv"; // Make sure this file exists
        _fileReaderOld = new FileReader(filePath);
        _fileReaderNew = new FileReader(filePath);
    }

    // Benchmark for the old version
    [Benchmark]
    public void Benchmark_ReadIntoCallbackOld()
    {
        _fileReaderOld.ReadIntoCallbackOld((transaction) => { /* No-op */ });
    }

    // Benchmark for the new version
    [Benchmark]
    public void Benchmark_ReadIntoCallback()
    {
        _fileReaderNew.ReadIntoCallback((transaction) => { /* No-op */ });
    }
}


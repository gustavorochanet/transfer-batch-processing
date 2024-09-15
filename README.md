# TransferBatch Processing Console App

This is a simple console application that processes transaction data from a CSV file, removes the highest transaction, calculates commissions for each account, and outputs the results to the console.

You can find a solution file (`.sln`) in the root of the project that can be opened in Visual Studio if preferred.

## Features

- **Load transactions from a CSV file**: Transaction data is streamed in and processed for better performance, especially for large files.
- **Store transactions in memory**: The `TransactionStorage` class stores transactions in memory, but the storage abstraction via the `ITransactionStorage` interface supports alternative storage backends (e.g., NoSQL databases).
- **Remove the highest transaction**: The application identifies and removes the transaction with the highest amount.
- **Calculate commissions for each account**: Commissions are calculated at a rate of 10% of the total transaction amount per account.
- **Output results with 2 decimal precision**: Results are displayed with proper formatting for precision.

## Requirements

To build and run this project, ensure that you have the following:

- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0) or later installed on your machine.

## How to Get the Code

### Option 1: Clone the Repository
Clone the project repository using the following command:

```
git clone https://github.com/gustavorochanet/transfer-batch-processing.git
```

Navigate to the project directory:

```
cd transfer-batch-processing
```

### Option 2: Download the ZIP File
Alternatively, you can get the code by downloading the ZIP file that I will provide via email.

## How to Build the Application
### 1. Restore Dependencies
Before building the application, restore the project dependencies by running the following command in the project root directory:

```
dotnet restore
```

This will download and install any necessary packages, including `Spectre.Console` and `Spectre.Console.CLI`, used to handle command-line arguments and UI update.

### 2. Build the Application
To build the application, run the following command:

```
dotnet build
```

This will compile the application and check for any compilation errors.

### 3. Run the Application
Once the application has been built successfully, you can run it with the following command:

```
dotnet run -- "path/to/your/csvfile.csv"
```

Make sure to replace `path/to/your/csvfile.csv` with the actual path to the CSV file that contains the transaction data.

Example:

```
dotnet run -- "C:\data\transactions.csv"
```

### 4. Building for Release
If you want to build a release version of the application, you can use the following command:

```
dotnet publish -c Release
```

This will generate an optimized version of the application in the `bin\Release\net8.0` folder.

## Sample Files
The project includes a `SampleFiles` directory with sample transaction data that is automatically copied to the output directory during the build process. You can use these files to test the application. After building the project, the sample files will be located in the output folder (`bin\Debug\net8.0\SampleFiles` or `bin\Release\net8.0\SampleFiles`).

## Running Unit Tests
To run the unit tests for the application, use the following command:

```
dotnet test
```

This will run all the unit tests defined in the test project and provide a summary of the test results.

## CSV File Format
The CSV file provided as input to the application should have the following structure:


- AccountID,TransactionID,TransactionAmount
- 1,TX001,150.00
- 1,TX002,200.50
- 2,TX003,350.75


Each row represents a transaction, and the file should not contain any header.

- `AccountID`: The ID of the account.
- `TransactionID`: The ID of the transaction.
- `TransactionAmount`: The amount for the transaction.


## Storage Mechanism Abstraction

The application abstracts transaction storage using the `ITransactionStorage` interface. Currently, an in-memory storage class (`TransactionStorage`) is implemented, which stores transactions in memory. However, this abstraction allows for the addition of other storage backends, such as NoSQL databases (e.g., MongoDB or CosmosDB), which would be more suitable for large-scale and distributed systems.

The interface defines the following methods:

- **GetTransactions**: Retrieves grouped transactions by account.
- **GetHigherTransaction**: Retrieves the transaction with the highest amount.
- **TransactionCount**: Tracks the number of transactions processed.


## Additional Tools

**Large CSV File Generator**: A separate console app has been added to the solution to generate large transaction files for performance testing purposes. This tool allows you to generate CSV files of any size, providing a useful way to stress-test the main application with large datasets.



## Performance Improvements and Method Comparisons

In the `FileReader` class, two methods exist for reading CSV files: `ReadIntoCallbackOld` and `ReadIntoCallback`.

### Why Are There Two Methods?

1. **ReadIntoCallbackOld**:
   - This method uses traditional string splitting (`Split`) and string processing techniques to parse each line of the file. While functional, it may not be as efficient, especially for very large files.
   
2. **ReadIntoCallback**:
   - This newer method is optimized for performance by avoiding unnecessary string allocations. It leverages `Span<T>` to operate directly on the memory where the strings are stored, which can reduce memory usage and improve overall processing speed for large datasets.

### Why Is One Version Faster?

- **Memory Optimization**: The newer version, `ReadIntoCallback`, reduces the overhead of creating new string arrays by operating on the raw string memory. This means fewer objects are allocated on the heap, leading to better memory management.
- **Span<T> Usage**: Using `Span<T>` allows for more efficient memory slicing without the need to split the string into arrays, which boosts performance, particularly when dealing with millions of lines.

### Benchmarking the Methods

To validate the performance improvement between these two methods, **BenchmarkDotNet** was integrated into the solution. This allows a detailed comparison of the execution times and memory usage of both methods.

The benchmark can be run to measure the performance of `ReadIntoCallbackOld` and `ReadIntoCallback` using a sample file. The results demonstrate which method is faster and more memory-efficient.


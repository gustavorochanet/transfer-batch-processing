# TransferBatch Processing Console App

This is a simple console application that processes transaction data from a CSV file, removes the highest transaction, calculates commissions for each account, and outputs the results to the console.

You can find a solution file (`.sln`) in the root of the project that can be opened in Visual Studio if preferred.

## Features

- **Load transactions from a CSV file**: Transaction data is streamed in and processed for better performance, especially for large files.
- **Store transactions in memory**: Transactions are stored using a `TransactionStorage` class that decouples file reading from processing.
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

This will download and install any necessary packages, including `System.CommandLine`, which is used to handle command-line arguments.

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



## Additional Tools

**Large CSV File Generator**: A separate console app has been added to the solution to generate large transaction files for performance testing purposes. This tool allows you to generate CSV files of any size, providing a useful way to stress-test the main application with large datasets.


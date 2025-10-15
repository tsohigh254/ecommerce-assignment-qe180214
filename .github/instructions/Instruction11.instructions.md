---
applyTo: '## Assignment 2: E-Commerce Web App with Auth, Product, Cart, and Order Management

**Objective**

Build a clothing e-commerce website where users can **register, log in, and manage products, cart, and orders**.

This project should be focusing on:
* [cite_start]**Register/Login RESTful API development** [cite: 1]
* [cite_start]**User authentication** [cite: 1]
* [cite_start]**CRUD operations** (Create, Read, Update, Delete) [cite: 1]
* [cite_start]**Cart and order management** [cite: 1]
* [cite_start]**Dynamic UI updates** [cite: 1]
* [cite_start]**Deployment to a free hosting platform** [cite: 1]

---

### Functional Requirements

#### 1. Authentication
Users must be able to:
* [cite_start]Register with email and password [cite: 1]
* [cite_start]Login to access their account [cite: 1]
* [cite_start]Logout [cite: 1]

**Authentication Method:** Use one of the following for authentication:
* [cite_start]**JWT** with your own API [cite: 1]
* [cite_start]**Supabase Auth** (recommended if using Supabase DB) [cite: 1]

**Access Control:**
* [cite_start]Only **logged-in users** can create, update, or delete products[cite: 1].
* [cite_start]**Unauthenticated users** can only view products[cite: 2].

#### 2. Product Model
Each product must include:
* [cite_start]**name** (string, required) [cite: 2]
* [cite_start]**description** (string, required) [cite: 2]
* [cite_start]**price** (number, required) [cite: 2]
* [cite_start]**image** (URL or upload, optional) [cite: 2]

#### 3. Cart Functionality
* [cite_start]Add product to cart (local state or database) [cite: 3]
* [cite_start]View cart items with quantity and total price [cite: 3]
* [cite_start]Remove item or update quantity [cite: 3]

#### 4. Order Management
* [cite_start]Place an order from the cart [cite: 4]
* [cite_start]Order must be saved in the database [cite: 4]

**Order model includes:**
* [cite_start]`userId` [cite: 4]
* [cite_start]`products` [cite: 4]
* [cite_start]`totalAmount` [cite: 4]
* [cite_start]`status` [cite: 4]

#### 5. Optional: Payment Flow (for bonus)
* [cite_start]Simulate payment or integrate **Stripe** / **Payos Checkout** (recommended for bonus)[cite: 4].
* [cite_start]After successful payment, update order status to **"paid"**[cite: 4].

---

### UI Requirements
The application's user interface must include:
* [cite_start]A **homepage** showing a list of products [cite: 3]
* [cite_start]A **product detail page** [cite: 3]
* [cite_start]A form to **create and update products** (only visible when logged in) [cite: 3]
* [cite_start]UI to **delete product** (only visible when logged in) [cite: 3]
* [cite_start]A **navigation menu** [cite: 3]
* [cite_start]**Register & Login Forms** [cite: 3]
* [cite_start]**Logout Button** [cite: 3]
* [cite_start]**Cart page**: list products added [cite: 3]
* [cite_start]**Checkout page**: confirm and place order [cite: 3]
* [cite_start]**Order history page**: list past orders [cite: 3]
* [cite_start][Optional] Payment success screen [cite: 3]

#### 7. Optional UI Features (for bonus marks)
* [cite_start]**Image upload** (using Cloudinary or similar) [cite: 3]
* [cite_start]**Search/filter products** [cite: 3]
* [cite_start]**Pagination** [cite: 3]

---

### 8. Deployment and Deliverables

#### Deployment
* [cite_start]Deploy the application (**Frontend + API**) to a **free hosting platform**[cite: 4].
* [cite_start]Ensure the database is accessible online (use free tiers of **PostgreSQL** or **MongoDB Atlas**)[cite: 4].
* [cite_start]Provide a **working link to the deployed website** + **Github repo**[cite: 5].

'
---
Provide project context and coding guidelines that AI should follow when generating code, answering questions, or reviewing changes.
================
CODE SNIPPETS
================
### Run .NET IoT Sense HAT Quickstart Script (Bash)

Source: https://learn.microsoft.com/en-us/dotnet/iot/quickstarts/sensehat_source=recommendations

Downloads and executes a shell script that installs the .NET SDK, clones the Sense HAT quickstart project from GitHub, builds it, and runs it. This script is essential for setting up and launching the project.

```bash
. <(wget -q -O - https://aka.ms/dotnet-iot-sensehat-quickstart)
```

--------------------------------

### Clone .NET 9 Example Solution using Git

Source: https://learn.microsoft.com/en-us/dotnet/aspire/get-started/add-aspire-existing-app_source=recommendations

This command clones the .NET 9 example solution named 'mslearn-dotnet-cloudnative-devops' and saves it locally as 'eShopLite'. Ensure Git is installed and configured on your system.

```bash
git clone https://github.com/MicrosoftDocs/mslearn-dotnet-cloudnative-devops.git eShopLite
```

--------------------------------

### Install OpenAI Packages

Source: https://learn.microsoft.com/en-us/dotnet/ai/quickstarts/build-chat-app_source=recommendations

Installs necessary NuGet packages for interacting with OpenAI services. This includes OpenAI client and configuration extensions.

```bash
dotnet add package OpenAI
dotnet add package Microsoft.Extensions.AI.OpenAI --prerelease
dotnet add package Microsoft.Extensions.Configuration
dotnet add package Microsoft.Extensions.Configuration.UserSecrets
```

--------------------------------

### Install Azure OpenAI Packages

Source: https://learn.microsoft.com/en-us/dotnet/ai/quickstarts/build-chat-app_source=recommendations

Installs necessary NuGet packages for interacting with Azure OpenAI services. This includes authentication, OpenAI client, and configuration extensions.

```bash
dotnet add package Azure.Identity
dotnet add package Azure.AI.OpenAI
dotnet add package Microsoft.Extensions.AI.OpenAI --prerelease
dotnet add package Microsoft.Extensions.Configuration
dotnet add package Microsoft.Extensions.Configuration.UserSecrets
```

--------------------------------

### Install Git on Raspberry Pi (Bash)

Source: https://learn.microsoft.com/en-us/dotnet/iot/quickstarts/sensehat_source=recommendations

Installs the latest version of Git on a Raspberry Pi using the Advanced Package Tool (apt). This involves updating package information and then installing the git package.

```bash
sudo apt update
sudo apt install git
```

--------------------------------

### Run .NET IoT Sense HAT Quickstart Script

Source: https://learn.microsoft.com/en-us/dotnet/iot/quickstarts/sensehat

This command downloads and executes a script from a provided URL. The script automates the installation of the .NET SDK, cloning of the Sense HAT quickstart project from GitHub, building the project, and running it to demonstrate Sense HAT functionality.

```Bash
. <(wget -q -O - https://aka.ms/dotnet-iot-sensehat-quickstart)

```

--------------------------------

### Run Ollama Model

Source: https://learn.microsoft.com/en-us/dotnet/ai/quickstarts/chat-local-model_source=recommendations

Starts the specified AI model using Ollama, making it ready for interaction. This command initializes the model and prepares it to receive prompts.

```bash
ollama run phi3:mini

```

--------------------------------

### Example User Query for Chat App

Source: https://learn.microsoft.com/en-us/dotnet/ai/get-started-app-chat-template_source=recommendations

This snippet demonstrates a typical user query to the chat application for retrieving specific information about a health plan. It serves as an input example for the chat interface.

```Text
What is included in my Northwind Health Plus plan that is not in standard?
```

--------------------------------

### Open Project in Visual Studio Code

Source: https://learn.microsoft.com/en-us/dotnet/ai/get-started-app-chat-template_source=recommendations

Opens the current directory in Visual Studio Code. This command assumes that Visual Studio Code is installed and configured in the system's PATH.

```bash
code .
```

--------------------------------

### Authenticate with Azure CLI

Source: https://learn.microsoft.com/en-us/dotnet/ai/get-started-app-chat-template

This command signs you into Azure using the Azure Developer CLI. It requires user interaction in a browser to complete the authentication flow. Ensure you have the Azure Developer CLI installed.

```bash
azd auth login

```

--------------------------------

### Verify Podman Installation

Source: https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/setup-tooling

These commands are used to confirm that Podman has been correctly installed and added to the system's PATH. 'which podman' locates the executable, and 'podman --version' displays the installed version, ensuring it's ready for use.

```bash
which podman
podman --version
```

--------------------------------

### Create and Navigate Project Directory

Source: https://learn.microsoft.com/en-us/dotnet/ai/get-started-app-chat-template_source=recommendations

Creates a new directory for the project and then changes the current directory to the newly created one. This is a standard way to set up a new project workspace.

```bash
mkdir my-intelligent-app && cd my-intelligent-app
```

--------------------------------

### Example Question for Deductible

Source: https://learn.microsoft.com/en-us/dotnet/ai/get-started-app-chat-template_source=recommendations

This is an example of a question that can be asked to retrieve deductible information. It's used to compare responses with and without semantic ranking.

```text
What is my deductible?
```

--------------------------------

### List .NET Aspire Templates using CLI

Source: https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/setup-tooling

This command lists all available .NET Aspire project templates installed on your system. It requires the .NET CLI to be installed and configured.

```dotnetcli
dotnet new list aspire

```

--------------------------------

### Example Prompt for MCP Server

Source: https://learn.microsoft.com/en-us/dotnet/ai/quickstarts/build-mcp-server

This is an example of a user prompt to interact with a configured MCP server, specifically to get city weather information. The prompt is a natural language question that GitHub Copilot can interpret to invoke the appropriate tool.

```text
What is the weather in Redmond?
```

--------------------------------

### Install Git on Raspberry Pi using APT

Source: https://learn.microsoft.com/en-us/dotnet/iot/quickstarts/sensehat

These commands update the package list and install the Git command-line tool on a Raspberry Pi using the Advanced Package Tool (APT). Ensure you have an internet connection and appropriate permissions to run these commands.

```Bash
sudo apt update
sudo apt install git

```

--------------------------------

### Verify Ollama Availability in Bash

Source: https://learn.microsoft.com/en-us/dotnet/ai/quickstarts/chat-local-model_source=recommendations

This command verifies if the Ollama executable is available in your system's PATH. It's a prerequisite to ensure Ollama is installed and accessible for running AI models locally. If Ollama is installed correctly, it will display a list of available commands.

```bash
ollama
```

--------------------------------

### Clone .NET 9 Example Solution using Git

Source: https://learn.microsoft.com/en-us/dotnet/aspire/get-started/add-aspire-existing-app

This command clones the .NET 9 example solution named 'eShopLite' from a GitHub repository. Ensure you have Git installed and are in the desired directory before execution.

```Bash
git clone https://github.com/MicrosoftDocs/mslearn-dotnet-cloudnative-devops.git eShopLite

```

--------------------------------

### Initialize project with Azure CLI

Source: https://learn.microsoft.com/en-us/dotnet/ai/get-started-app-chat-template

This command initializes a new project by cloning a specified template repository. It sets up the project structure and necessary configuration files for your application. The `-t` flag specifies the template.

```bash
azd init -t azure-search-openai-demo-csharp

```

--------------------------------

### C# Hello World Program Structure

Source: https://learn.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/get-started/syntax-analysis_source=recommendations

Demonstrates the basic structure of a 'Hello World' program in C#, illustrating elements like using directives, namespace, class, and method declarations. This code is a foundational example for understanding the Syntax API's representation of code.

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
```

--------------------------------

### Start Ollama Server

Source: https://learn.microsoft.com/en-us/dotnet/ai/quickstarts/ai-templates

This command starts the Ollama local server, which is required for the AI Chat Web App to function. Ensure Ollama is installed and running before proceeding.

```bash
ollama serve
```

--------------------------------

### Write Your First C# Code - Basic Syntax

Source: https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts

A training module designed to help beginners get started with C# by writing code examples and learning the fundamental syntax of the language.

```csharp
using System;

public class HelloWorld
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Hello, C# World!");
    }
}
```

--------------------------------

### Create .NET Aspire Starter App with Redis using .NET CLI

Source: https://learn.microsoft.com/en-us/dotnet/aspire/get-started/build-your-first-aspire-app

Creates a new .NET Aspire Starter Application project named 'AspireSample' and configures it to use Redis for caching. This command utilizes the installed .NET Aspire templates.

```bash
dotnet new aspire-starter --use-redis-cache --output AspireSample

```

--------------------------------

### Run .NET Aspire App Project

Source: https://learn.microsoft.com/en-us/dotnet/aspire/get-started/build-your-first-aspire-app

This command executes a specified .NET Aspire application project. It is used to start the application for local testing and development. The command requires the path to the project file as an argument. Successful execution will launch the application.

```bash
dotnet run --project AspireSample/AspireSample.AppHost

```

--------------------------------

### Initialize Project with Azure Developer CLI

Source: https://learn.microsoft.com/en-us/dotnet/ai/get-started-app-chat-template_source=recommendations

Initializes a new project by cloning a specified GitHub repository using the Azure Developer CLI. This command sets up the project structure and necessary files for development.

```bash
azd init -t azure-search-openai-demo-csharp
```

--------------------------------

### Create a .NET Aspire starter app using .NET CLI

Source: https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/aspire-sdk-templates

Creates a .NET Aspire starter application, which includes a full solution with a sample UI and a backing API. This template provides a ready-to-run example to explore Aspire's capabilities.

```bash
dotnet new aspire-starter

```

--------------------------------

### Create and navigate to project directory

Source: https://learn.microsoft.com/en-us/dotnet/ai/get-started-app-chat-template

These commands create a new directory for your project and then change the current directory to the newly created one. This is a standard practice for organizing project files. The `mkdir` command creates the directory, and `cd` changes into it.

```bash
mkdir my-intelligent-app && cd my-intelligent-app

```

--------------------------------

### Write your first C# code - Training Module

Source: https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts_source=recommendations

A training module designed to help beginners get started with C# by writing code examples. It focuses on learning the basic syntax of the C# language.

```csharp
using System;

// A simple C# program to print a message.
public class HelloWorld
{
    public static void Main(string[] args)
    {
        // This line prints "Hello, C# World!" to the console.
        Console.WriteLine("Hello, C# World!");
    }
}

```

--------------------------------

### Initialize SpeechSynthesizer and Select Voice (.NET)

Source: https://learn.microsoft.com/en-us/dotnet/api/system.speech_view=netframework-4.8

Demonstrates initializing a SpeechSynthesizer instance and selecting a specific installed voice. It shows how to get information about available voices and configure the synthesizer to use a different one.

```C#
using System.Speech.Synthesis;

// Initialize the SpeechSynthesizer to the default voice.
SpeechSynthesizer synth = new SpeechSynthesizer();

// Configure the synthesizer to the speaking (Italian) voice.
// The constructor could also be used:
// synth.SetOutputToWaveFile(@"output.wav");

// Select a voice that matches the specified culture.
synth.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Adult, 0, new System.Globalization.CultureInfo("it-IT"));

// Speak a string synchronously.
synth.Speak("Hello, world!");

// Speak a string asynchronously.
synth.SpeakAsync("Good morning!");

// Speak SSML synchronously.
synth.SpeakSsml("<speak>Hello from SSML!</speak>");

// Speak SSML asynchronously.
synth.SpeakSsmlAsync("<speak>Another SSML example.</speak>");

// Example of setting output to a file
synth.SetOutputToWaveFile("output.wav");

// Example of appending text to a prompt builder
PromptBuilder pb = new PromptBuilder();
pb.AppendText("This is a prompt.");
synth.Speak(pb);

// Example of adding a lexicon
// synth.AddLexicon("myLexicon.xml");

// Example of starting a voice with specific styles
PromptBuilder pbStyle = new PromptBuilder();
pbStyle.StartVoice("Microsoft Zira Desktop", new System.Globalization.CultureInfo("en-US"));
pbStyle.AppendText("Speaking with Zira.");
pbStyle.EndVoice();
synth.Speak(pbStyle);
```

--------------------------------

### Sentiment Analysis Response Example

Source: https://learn.microsoft.com/en-us/dotnet/ai/quickstarts/structured-output_source=recommendations

This is an example of the output generated by the C# code for sentiment analysis. It shows the 'ResponseText' from the model and the 'ReviewSentiment' detected.

```output
Response text: Certainly, I have analyzed the sentiment of the review you provided.
Sentiment: Neutral
```

--------------------------------

### Delete Azure Resources with Azure Developer CLI

Source: https://learn.microsoft.com/en-us/dotnet/ai/get-started-app-chat-template_source=recommendations

This command removes all Azure resources created during the tutorial and deletes the source code. Ensure you have the Azure Developer CLI installed.

```bash
azd down --purge
```

--------------------------------

### C# Method Declarations and Access Modifiers

Source: https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/methods

Demonstrates how to declare methods with different access levels (`public`, `protected`, `abstract`) and modifiers (`virtual`, `abstract`) within a C# class. It shows examples of methods for starting an engine, adding gas, driving, and getting top speed, with placeholder statements.

```csharp
abstract class Motorcycle
{
    // Anyone can call this.
    public void StartEngine() {/* Method statements here */ } 

    // Only derived classes can call this.
    protected void AddGas(int gallons) { /* Method statements here */ } 

    // Derived classes can override the base class implementation.
    public virtual int Drive(int miles, int speed) { /* Method statements here */ return 1; } 

    // Derived classes must implement this.
    public abstract double GetTopSpeed();
}
```

--------------------------------

### C# Basic Console Application Setup

Source: https://learn.microsoft.com/en-us/dotnet/csharp/asynchronous-programming/start-multiple-async-tasks-and-process-them-as-they-complete_source=recommendations

This snippet sets up a basic C# console application structure within the ProcessTasksAsTheyFinish namespace. It includes the Main method, which is the entry point of the application. This serves as the initial template for the example.

```csharp
using System.Diagnostics;

namespace ProcessTasksAsTheyFinish;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello World!");
    }
}
```

--------------------------------

### Create New .NET Console App

Source: https://learn.microsoft.com/en-us/dotnet/ai/quickstarts/chat-local-model_source=recommendations

Creates a new .NET console application in a specified directory. This command sets up the basic project structure for a new application.

```bash
dotnet new console -o LocalAI

```

--------------------------------

### Start Simulation and Gather Baseline Data in C#

Source: https://learn.microsoft.com/en-us/dotnet/csharp/advanced-topics/performance/ref-tutorial

This C# code snippet initializes a room, takes initial measurements, and prints them to the console. It simulates the start of an environment monitoring process, establishing a baseline for subsequent data analysis. Dependencies include System and custom Room class.

```C#
Console.WriteLine("Press <return> to start simulation");
Console.ReadLine();
var room = new Room("gallery");
var r = new Random();

int counter = 0;

room.TakeMeasurements(
    m =>
    {
        Console.WriteLine(room.Debounce);
        Console.WriteLine(room.Average);
        Console.WriteLine();
        counter++;
        return counter < 20000;
    });

```

--------------------------------

### Create OpenAI and Azure OpenAI Clients in C#

Source: https://learn.microsoft.com/en-us/dotnet/ai/quickstarts/create-assistant_source=recommendations

Initializes clients for interacting with OpenAI and Azure OpenAI services in a C# .NET application. It demonstrates how to set up clients using API keys or Azure identity credentials and obtain assistant and file clients.

```csharp
using OpenAI;
using OpenAI.Assistants;
using OpenAI.Files;
using Azure.AI.OpenAI;
using Azure.Identity;

// Create the OpenAI client
OpenAIClient openAIClient = new("your-apy-key");

// For Azure OpenAI, use the following client instead:
AzureOpenAIClient azureAIClient = new(
        new Uri("your-azure-openai-endpoint"),
        new DefaultAzureCredential());

#pragma warning disable OPENAI001
AssistantClient assistantClient = openAIClient.GetAssistantClient();
OpenAIFileClient fileClient = openAIClient.GetOpenAIFileClient();

```

--------------------------------

### Create C# syntax tree and get root node in C#

Source: https://learn.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/get-started/syntax-analysis_source=recommendations

Parses the 'programText' string into a C# syntax tree and retrieves the root node (CompilationUnitSyntax). This is the starting point for analyzing the code structure.

```csharp
SyntaxTree tree = CSharpSyntaxTree.ParseText(programText);
CompilationUnitSyntax root = tree.GetCompilationUnitRoot();
```

--------------------------------

### Verify .NET SDK Installation (CLI)

Source: https://learn.microsoft.com/en-us/dotnet/maui/get-started/installation_view=net-maui-9

This command verifies the installation of the .NET SDK by displaying its version. It's a crucial step after installing the SDK on any platform.

```.NET CLI
dotnet --version
```

--------------------------------

### C# Indexers Example

Source: https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/types/boxing-and-unboxing

Demonstrates how C# indexers allow instances of a class or struct to be accessed like arrays. This enables getting or setting indexed values without specifying the instance member or type, providing a convenient way to access collections or internal data.

```csharp
using System;

public class MyCollection {
    private string[] _items = new string[10];

    // Indexer declaration
    public string this[int index] {
        get {
            if (index < 0 || index >= _items.Length) {
                throw new IndexOutOfRangeException("Index out of bounds.");
            }
            return _items[index] ?? "<empty>"; // Return "<empty>" if null
        }
        set {
            if (index < 0 || index >= _items.Length) {
                throw new IndexOutOfRangeException("Index out of bounds.");
            }
            _items[index] = value;
        }
    }

    // Property to get the size of the collection
    public int Count {
        get { return _items.Length; }
    }

    public static void Main(string[] args) {
        MyCollection collection = new MyCollection();

        // Using the indexer to set values
        collection[0] = "Apple";
        collection[1] = "Banana";
        collection[5] = "Cherry";

        // Using the indexer to get values
        Console.WriteLine($"Item at index 0: {collection[0]}"); // Output: Apple
        Console.WriteLine($"Item at index 1: {collection[1]}"); // Output: Banana
        Console.WriteLine($"Item at index 5: {collection[5]}"); // Output: Cherry
        Console.WriteLine($"Item at index 3: {collection[3]}"); // Output: <empty>

        // Attempting to access out of bounds (will throw exception)
        try {
            collection[10] = "Date";
        } catch (IndexOutOfRangeException ex) {
            Console.WriteLine(ex.Message); // Output: Index out of bounds.
        }
    }
}
```

--------------------------------

### Microsoft.AspNetCore Namespace

Source: https://learn.microsoft.com/en-us/dotnet/api_view=aspnetcore-5

Provides types that enable getting started with building an ASP.NET app with opinionated defaults.

```APIDOC
## ASP.NET Core API Reference v5.0 - Microsoft.AspNetCore Namespace

### Description
Provides types that enable getting started with building an ASP.NET app with opinionated defaults.

### Method
N/A

### Endpoint
N/A

### Parameters
N/A

### Request Example
N/A

### Response
N/A

### Response Example
N/A
```

--------------------------------

### Microsoft.AspNetCore Namespace

Source: https://learn.microsoft.com/en-us/dotnet/api_view=aspnetcore-2

Provides types that enable getting started with building an ASP.NET app with opinionated defaults.

```APIDOC
## Microsoft.AspNetCore Namespace

### Description
Provides types that enable getting started with building an ASP.NET app with opinionated defaults.

### Method
N/A (Namespace Overview)

### Endpoint
N/A

### Parameters
N/A

### Request Example
N/A

### Response
N/A
```

--------------------------------

### .NET Aspire AppHost Project File Configuration

Source: https://learn.microsoft.com/en-us/dotnet/aspire/get-started/build-your-first-aspire-app

This XML snippet configures a .NET Aspire AppHost project. It specifies the SDK, target framework, and includes necessary package references for Aspire hosting. The project file defines dependencies on other projects within the solution.

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <Sdk Name="Aspire.AppHost.Sdk" Version="9.5.0" />

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net9.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <UserSecretsId>2aa31fdb-0078-4b71-b953-d23432af8a36</UserSecretsId>
  </PropertyGroup>

  <ItemGroup>
    <ProjectReference Include="..\AspireSample.ApiService\AspireSample.ApiService.csproj" />
    <ProjectReference Include="..\AspireSample.Web\AspireSample.Web.csproj" />
  </ItemGroup>

  <ItemGroup>
    <PackageReference Include="Aspire.Hosting.AppHost" Version="9.5.0" />
    <PackageReference Include="Aspire.Hosting.Redis" Version="9.5.0" />
  </ItemGroup>

</Project>
```

--------------------------------

### Microsoft.AspNetCore Namespace

Source: https://learn.microsoft.com/en-us/dotnet/api_view=aspnetcore-6

Provides types that enable getting started with building an ASP.NET app with opinionated defaults.

```APIDOC
## Microsoft.AspNetCore Namespace

### Description
Provides types that enable getting started with building an ASP.NET app with opinionated defaults.

### Method
N/A

### Endpoint
N/A

### Parameters
N/A

### Request Example
N/A

### Response
N/A

### Response Example
N/A
```

--------------------------------

### AppHost Service Orchestration Logic (C#)

Source: https://learn.microsoft.com/en-us/dotnet/aspire/get-started/quickstart-build-your-first-aspire-app

Defines the application's runtime orchestration using the .NET Aspire AppHost. It configures services, their dependencies, external endpoints, and startup order, leveraging a familiar builder pattern.

```csharp
var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var apiService = builder.AddProject<Projects.AspireSample_ApiService>("apiservice");

builder.AddProject<Projects.AspireSample_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
```

--------------------------------

### Microsoft.AspNetCore Namespace

Source: https://learn.microsoft.com/en-us/dotnet/api_view=aspnetcore-8

Provides types that enable getting started with building an ASP.NET app with opinionated defaults.

```APIDOC
## Microsoft.AspNetCore Namespace

### Description
Provides types that enable getting started with building an ASP.NET app with opinionated defaults.

### Method
N/A

### Endpoint
N/A

### Parameters
N/A

### Request Example
N/A

### Response
N/A
```

--------------------------------

### Create New .NET Console App

Source: https://learn.microsoft.com/en-us/dotnet/ai/quickstarts/build-chat-app_source=recommendations

Initializes a new .NET console application in a specified directory. This command is the first step in setting up the project structure.

```dotnetcli
dotnet new console -o ChatAppAI
```

--------------------------------

### Microsoft.AspNetCore Namespace

Source: https://learn.microsoft.com/en-us/dotnet/api_view=aspnetcore-10

Provides types that enable getting started with building an ASP.NET app with opinionated defaults.

```APIDOC
## ASP.NET Core Namespace

### Description
Provides types that enable getting started with building an ASP.NET app with opinionated defaults.

### Method
N/A

### Endpoint
N/A

### Parameters
N/A

### Request Example
N/A

### Response
N/A
```

--------------------------------

### Pull Ollama Model

Source: https://learn.microsoft.com/en-us/dotnet/ai/quickstarts/chat-local-model_source=recommendations

Downloads the specified AI model from the Ollama registry. This command ensures the model is available locally before attempting to run it.

```bash
ollama pull phi3:mini

```

--------------------------------

### UseStartup<TStartup>(IWebHostBuilder, Func<WebHostBuilderContext,TStartup>)

Source: https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.hosting.webhostbuilderextensions_view=aspnetcore-9

Specifies a factory that creates the startup instance to be used by the web host.

```APIDOC
## UseStartup<TStartup>(IWebHostBuilder, Func<WebHostBuilderContext,TStartup>)

### Description
Specify a factory that creates the startup instance to be used by the web host.

### Method
Extension Method (C#)

### Endpoint
N/A (This is a method extension, not a REST endpoint)

### Parameters
#### Type Parameters
* **TStartup** - The type of the startup class.

#### Parameters
* **hostBuilder** (IWebHostBuilder) - Required - The IWebHostBuilder to configure.
* **startupFactory** (Func<WebHostBuilderContext,TStartup>) - Required - A delegate that specifies a factory for the startup class.

### Remarks
When in a trimmed app, all public methods of `TStartup` are preserved. This should match the Startup type directly (and not a base type).

### Applies to
* ASP.NET Core 5.0, 6.0, 7.0, 8.0, 9.0, 10.0
```

--------------------------------

### Microsoft.AspNetCore Namespace Overview

Source: https://learn.microsoft.com/en-us/dotnet/api/microsoft_view=aspnetcore-7

Provides types that enable getting started with building an ASP.NET app with opinionated defaults.

```APIDOC
## Namespace: Microsoft.AspNetCore

### Description
Provides types that enable getting started with building an ASP.NET app with opinionated defaults.

### Key Classes
*   **WebHost**: Provides convenience methods for creating instances of `IWebHost` and `IWebHostBuilder` with pre-configured defaults.

### Remarks
For more information about hosting ASP.NET Core apps, see ASP.NET Core Web Host.
```

--------------------------------

### Chain of Thought Prompting with Examples (C#)

Source: https://learn.microsoft.com/en-us/dotnet/ai/conceptual/chain-of-thought-prompting

This C# code snippet illustrates using examples to guide chain of thought prompting. By providing a structured example with formatting cues, the model is shown how to break down tasks and present step results, facilitating clearer reasoning and output.

```csharp
prompt= """
        Instructions: Compare the pros and cons of EVs and petroleum-fueled vehicles.

        Differences between EVs and petroleum-fueled vehicles:
        - 

        Differences ordered according to overall impact, highest-impact first: 
        1. 
        
        Summary of vehicle type differences as pros and cons:
        Pros of EVs
        1.
        Pros of petroleum-fueled vehicles
        1. 
        """
```

--------------------------------

### List .NET Aspire project templates using dotnet CLI

Source: https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/setup-tooling_source=recommendations

This command verifies that the .NET Aspire templates are installed by listing all available templates matching the 'aspire' keyword. The output shows template names, short names, languages, and tags. Ensure you have the .NET SDK installed.

```dotnetcli
dotnet new list aspire
```

```dotnetcli
dotnet new list aspire
These templates matched your input: 'aspire'

Template Name                      Short Name              Language  Tags
---------------------------------  ----------------------  --------  -------------------------------------------------------
.NET Aspire AppHost               aspire-apphost          [C#]      Common/.NET Aspire/Cloud
.NET Aspire Empty App              aspire                  [C#]      Common/.NET Aspire/Cloud/Web/Web API/API/Service
.NET Aspire Service Defaults       aspire-servicedefaults  [C#]      Common/.NET Aspire/Cloud/Web/Web API/API/Service
.NET Aspire Starter App            aspire-starter          [C#]      Common/.NET Aspire/Blazor/Web/Web API/API/Service/Cloud
.NET Aspire Test Project (MSTest)  aspire-mstest           [C#]      Common/.NET Aspire/Cloud/Web/Web API/API/Service/Test
.NET Aspire Test Project (NUnit)   aspire-nunit            [C#]      Common/.NET Aspire/Cloud/Web/Web API/API/Service/Test
.NET Aspire Test Project (xUnit)   aspire-xunit            [C#]      Common/.NET Aspire/Cloud/Web/Web API/API/Service/Test
```

--------------------------------

### Microsoft.AspNetCore Namespace Overview

Source: https://learn.microsoft.com/en-us/dotnet/api/microsoft_view=aspnetcore-2

Provides types that enable getting started with building an ASP.NET app with opinionated defaults.

```APIDOC
## Namespace: Microsoft.AspNetCore

### Description
Provides types that enable getting started with building an ASP.NET app with opinionated defaults.

### Remarks
For more information about hosting ASP.NET Core apps, see ASP.NET Core Web Host.
```

--------------------------------

### C# Required Property Usage Examples

Source: https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/properties

Provides examples of using C# required properties, showing valid initialization through a constructor or object initializer, and highlighting a compilation error for failing to initialize.

```csharp
var aPerson = new Person("John");
aPerson = new Person { FirstName = "John"};
// Error CS9035: Required member `Person.FirstName` must be set:
//aPerson2 = new Person();

```

--------------------------------

### C# Expression-Bodied Get and Set Accessors

Source: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/get

This C# example showcases expression-bodied members for both `get` and `set` accessors in the `Seconds` property of the `TimePeriod2` class. This concise syntax is suitable when the accessor body consists of a single expression, such as returning or assigning a value to a backing field.

```csharp
class TimePeriod2
{
    private double _seconds;

    public double Seconds
    {
        get => _seconds;
        set => _seconds = value;
    }
}
```

--------------------------------

### ASP.NET Core Namespace

Source: https://learn.microsoft.com/en-us/dotnet/api_view=aspnetcore-9

Provides types that enable getting started with building an ASP.NET app with opinionated defaults.

```APIDOC
## Microsoft.AspNetCore Namespace

### Description
Provides types that enable getting started with building an ASP.NET app with opinionated defaults.

### Method
N/A (Namespace Overview)

### Endpoint
N/A

### Parameters
N/A

### Request Example
N/A

### Response
N/A

```

--------------------------------

### Run Aspire AppHost and View Output

Source: https://learn.microsoft.com/en-us/dotnet/aspire/cli/overview

The `aspire run` command executes the AppHost project in development mode. It sets up the Aspire environment, builds and launches defined resources, starts the web dashboard, and displays a list of available endpoints. The command searches for an AppHost project in the current directory or its subdirectories. It also handles the installation and trust of local hosting certificates.

```text
Dashboard:  https://localhost:17178/login?t=17f974bf68e390b0d4548af8d7e38b65

    Logs:  /home/vscode/.aspire/cli/logs/apphost-1295-2025-07-14-18-16-13.log

```

--------------------------------

### Create Basic .NET Console Application (C#)

Source: https://learn.microsoft.com/en-us/dotnet/csharp/asynchronous-programming/start-multiple-async-tasks-and-process-them-as-they-complete

This C# code snippet demonstrates the basic structure of a .NET Core console application. It includes the necessary `using` directive and a simple `Main` method that prints 'Hello World!' to the console. This serves as the initial setup for the example.

```csharp
using System;

namespace ProcessTasksAsTheyFinish;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello World!");
    }
}

```

--------------------------------

### C# Example of Read-Write Property with Get and Set Accessors

Source: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/classes

Illustrates a read-write property in C# with both get and set accessors. The get accessor retrieves the value of a private field, while the set accessor updates the field and triggers additional actions, such as repainting a UI element.

```C#
public class Button : Control
{
    private string caption;

    public string Caption
    {
        get => caption;
        set
        {
            if (caption != value)
            {
                caption = value;
                Repaint();
            }
        }
    }

    public override void Paint(Graphics g, Rectangle r)
    {
        // Painting code goes here
    }
}

```

--------------------------------

### MCP Server Response Example (Output)

Source: https://learn.microsoft.com/en-us/dotnet/ai/quickstarts/build-mcp-server

An example of the output received from the MCP server after a successful tool execution. This demonstrates the server's ability to respond to prompts and execute the requested functionality, such as generating a random number.

```output
Your random number is 42.

```

--------------------------------

### Create .NET Console App

Source: https://learn.microsoft.com/en-us/dotnet/ai/quickstarts/create-assistant_source=recommendations

Creates a new .NET console application in a specified directory. This is the initial step for building a .NET AI assistant project.

```dotnetcli
dotnet new console -o AIAssistant
```

--------------------------------

### nameof Operator Usage Examples - C#

Source: https://learn.microsoft.com/en-us/dotnet/_csharplang/proposals/csharp-8.0/null-coalescing-assignment

Illustrates various uses of the 'nameof' operator in C#. It shows how to get the names of namespaces, types, members, and generic types, along with examples of invalid usage.

```C#
using TestAlias = System.String;

class Program
{
    static void Main()
    {
        var point = (x: 3, y: 4);

        string n1 = nameof(System);                      // "System"
        string n2 = nameof(System.Collections.Generic);  // "Generic"
        string n3 = nameof(point);                       // "point"
        string n4 = nameof(point.x);                     // "x"
        string n5 = nameof(Program);                     // "Program"
        string n6 = nameof(System.Int32);                // "Int32"
        string n7 = nameof(TestAlias);                   // "TestAlias"
        string n8 = nameof(List<int>);                   // "List"
        string n9 = nameof(Program.InstanceMethod);      // "InstanceMethod"
        string n10 = nameof(Program.GenericMethod);      // "GenericMethod"
        string n11 = nameof(Program.NestedClass);        // "NestedClass"

        // Invalid
        // string x1 = nameof(List<>);            // Empty type argument list
        // string x2 = nameof(List<T>);           // T is not in scope
        // string x3 = nameof(GenericMethod<>);   // Empty type argument list
        // string x4 = nameof(GenericMethod<T>);  // T is not in scope
        // string x5 = nameof(int);               // Keywords not permitted
        // Type arguments not permitted for method group
        // string x6 = nameof(GenericMethod<Program>); 
    }

    void InstanceMethod() { }

    void GenericMethod<T>()
    {
        string n1 = nameof(List<T>); // "List"
        string n2 = nameof(T);       // "T"
    }

    class NestedClass { }
}
```

--------------------------------

### Create .NET Aspire Starter App using .NET CLI

Source: https://learn.microsoft.com/en-us/dotnet/aspire/get-started/build-your-first-aspire-app_tabs=visual-studio

Creates a new .NET Aspire starter application with specified configurations, such as using a Redis cache. The `--output` flag specifies the directory for the new project.

```dotnetcli
dotnet new aspire-starter --use-redis-cache --output AspireSample
```

--------------------------------

### C# Interactive 'Hello World' Example

Source: https://learn.microsoft.com/en-us/dotnet/csharp/tour-of-csharp/tutorials/hello-world_source=recommendations

This code snippet demonstrates a basic 'Hello World' program in C#. It's designed for interactive execution within a browser environment, allowing users to see the direct output of compiled and run code. This is part of an introductory tutorial for learning C#.

```csharp
using System;

public class HelloWorld
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
    }
}
```

--------------------------------

### Complete Orleans Client Project Example (C#)

Source: https://learn.microsoft.com/en-us/dotnet/aspire/frameworks/orleans

This complete C# example illustrates a .NET Aspire Orleans client project. It sets up Azure Table Storage for clustering, configures the Orleans client, and defines endpoints for interacting with a `CounterGrain` using HTTP GET and POST requests.

```csharp
using OrleansContracts;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddKeyedAzureTableClient("clustering");
builder.UseOrleansClient();

var app = builder.Build();

app.MapGet("/counter/{grainId}", async (IClusterClient client, string grainId) =>
{
    var grain = client.GetGrain<ICounterGrain>(grainId);
    return await grain.Get();
});

app.MapPost("/counter/{grainId}", async (IClusterClient client, string grainId) =>
{
    var grain = client.GetGrain<ICounterGrain>(grainId);
    return await grain.Increment();
});

app.UseFileServer();

await app.RunAsync();
```

--------------------------------

### UseStartup<TStartup>(IWebHostBuilder, Func<WebHostBuilderContext,TStartup>)

Source: https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.hosting.webhostbuilderextensions_view=aspnetcore-9

Specifies a factory that creates the startup instance to be used by the web host. This overload allows for custom instantiation logic for the startup type.

```APIDOC
## UseStartup<TStartup>(IWebHostBuilder, Func<WebHostBuilderContext,TStartup>)

### Description
Specify a factory that creates the startup instance to be used by the web host.

### Method
POST

### Endpoint
/configure-startup-factory

### Parameters
#### Path Parameters
None

#### Query Parameters
None

#### Request Body
- **hostBuilder** (IWebHostBuilder) - Required - The IWebHostBuilder to configure.
- **startupFactory** (Func<WebHostBuilderContext,TStartup>) - Required - A factory function that creates the startup instance.
```

--------------------------------

### AI Prompt and Response for Shortening Text (.NET)

Source: https://learn.microsoft.com/en-us/dotnet/ai/quickstarts/chat-local-model_source=recommendations

This snippet demonstrates an AI prompt designed to shorten a previous AI response, followed by the AI's abbreviated output. It highlights the AI's ability to refine its verbosity based on chat history and specific instructions, using .NET as the context.

```text
Your prompt:
Shorten the length of each item in the previous response.

AI Response:
 **Cross-platform Capabilities:** .NET allows building for various operating systems
through platforms like .NET Core, promoting accessibility (Windows, Linux, macOS).

**Extensive Ecosystem:** Offers a vast library selection via NuGet and tools for web
(.NET Framework), mobile development (.NET MAUI), IoT, AI, providing rich
capabilities to developers.

**Type Safety &amp; Reliability:** .NET's CLI model enforces strong typing and automatic
garbage collection, mitigating runtime errors, thus enhancing application stability.
```

--------------------------------

### Navigate to Project Directory

Source: https://learn.microsoft.com/en-us/dotnet/ai/quickstarts/build-chat-app_source=recommendations

Changes the current directory to the newly created application folder. This is essential for running subsequent commands within the project context.

```bash
cd ChatAppAI
```

--------------------------------

### C# Example: Object and Collection Initializers

Source: https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/object-and-collection-initializers

This example demonstrates combining object and collection initializers in C#. It shows how to initialize a single object with properties and a list of objects using collection initializers. It also illustrates collection expressions for combining lists.

```C#
public class InitializationSample
{
    public class Cat
    {
        // Automatically implemented properties.
        public int Age { get; set; }
        public string? Name { get; set; }

        public Cat() { }

        public Cat(string name)
        {
            Name = name;
        }
    }

    public static void Main()
    {
        Cat cat = new Cat { Age = 10, Name = "Fluffy" };
        Cat sameCat = new Cat("Fluffy"){ Age = 10 };

        List<Cat> cats = new List<Cat>
        {
            new Cat { Name = "Sylvester", Age = 8 },
            new Cat { Name = "Whiskers", Age = 2 },
            new Cat { Name = "Sasha", Age = 14 }
        };

        List<Cat?> moreCats = new List<Cat?>
        {
            new Cat { Name = "Furrytail", Age = 5 },
            new Cat { Name = "Peaches", Age = 4 },
            null
        };

        List<Cat> allCats = [.. cats, new Cat { Name = "Łapka", Age = 5 }, cat, .. moreCats];

        // Display results.
        foreach (Cat? c in allCats)
        {
            if (c != null)
            {
                System.Console.WriteLine(c.Name);
            }
            else
            {
                System.Console.WriteLine("List element has null value.");
            }
        }
    }
    // Output:
    // Sylvester
    // Whiskers
    // Sasha
    // Łapka
    // Fluffy
    // Furrytail
    // Peaches
    // List element has null value.
}

```

--------------------------------

### C# Prompt Example for Structuring AI Output

Source: https://learn.microsoft.com/en-us/dotnet/ai/conceptual/prompt-engineering-dotnet

This C# code snippet demonstrates how to structure a prompt for an AI model using instructions, categories, and specific cues to format the output. It includes example text for presidential accomplishments and how cues like 'DOMESTIC POLICY' and '- George Washington:' guide the AI's response structure.

```csharp
prompt = """
Instructions: Summarize US Presidential accomplishments, grouped by category.
Categories: Domestic Policy, US Economy, Foreign Affairs, Space Exploration.
Accomplishments: George Washington
First president of the United States.
First president to have been a military veteran.
First president to be elected to a second term in office.
First president to receive votes from every presidential elector in an election.
First president to fill the entire body of the United States federal judges; including the Supreme Court.
First president to be declared an honorary citizen of a foreign country, and an honorary citizen of France.
John Adams ...  /// Text truncated

DOMESTIC POLICY
- George Washington: 
- John Adams: 
""";

```

--------------------------------

### C# Example: Using #pragma checksum with a Specific Checksum

Source: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/preprocessor-directives

Provides a C# code example illustrating the practical use of the #pragma checksum directive within a class. It shows how to specify the filename, a GUID for the hash algorithm, and the checksum bytes.

```csharp
class TestClass
{
    static int Main()
    {
        #pragma checksum "file.cs" "{406EA660-64CF-4C82-B6F0-42D48172A799}" "ab007f1d23d9" // New checksum
    }
}

```

--------------------------------

### Create .NET Console App using .NET CLI

Source: https://learn.microsoft.com/en-us/dotnet/iot/tutorials/lcd-display

Creates a new .NET Console Application named 'LcdTutorial' and navigates into its directory. This is the initial step for project setup.

```bash
dotnet new console -o LcdTutorial
cd LcdTutorial
```

--------------------------------

### Query .NET Framework registry in C#

Source: https://learn.microsoft.com/en-us/dotnet/framework/install/how-to-determine-which-versions-are-installed

This C# code snippet demonstrates how to open the appropriate registry key for .NET Framework setup and read the 'Release' value to determine the installed version. It includes a helper function to translate the release key value into a human-readable version string. It handles cases where the .NET Framework might not be installed or detected.

```csharp
const string subkey = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\";

using (RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))
using (RegistryKey ndpKey = baseKey.OpenSubKey(subkey))
{
    if (ndpKey != null && ndpKey.GetValue("Release") != null)
        Console.WriteLine($".NET Framework Version: {CheckFor45PlusVersion((int)ndpKey.GetValue("Release"))}");
    else
        Console.WriteLine(".NET Framework Version 4.5 or later is not detected.");
}

// Checking the version using >= enables forward compatibility.
string CheckFor45PlusVersion(int releaseKey)
{
    if (releaseKey >= 533320) return "4.8.1 or later";
    if (releaseKey >= 528040) return "4.8";
    if (releaseKey >= 461808) return "4.7.2";
    if (releaseKey >= 461308) return "4.7.1";
    if (releaseKey >= 460798) return "4.7";
    if (releaseKey >= 394802) return "4.6.2";
    if (releaseKey >= 394254) return "4.6.1";
    if (releaseKey >= 393295) return "4.6";
    if (releaseKey >= 379893) return "4.5.2";
    if (releaseKey >= 378675) return "4.5.1";
    if (releaseKey >= 378389) return "4.5";

    // This code should never execute. A non-null release key should mean
    // that 4.5 or later is installed.
    return "No 4.5 or later version detected";
}

```

--------------------------------

### Configure Background Task Services in Program.cs (C#)

Source: https://learn.microsoft.com/en-us/dotnet/core/extensions/queue-service

This C# code demonstrates the setup for background tasks in the `Program.cs` file. It uses `HostApplicationBuilder` to configure services, registering `MonitorLoop` as a singleton, `QueuedHostedService` as a hosted service, and `IBackgroundTaskQueue` with a configurable capacity. It then retrieves the `MonitorLoop` service and starts it before running the host.

```csharp
using App.QueueService;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Services.AddSingleton<MonitorLoop>();
builder.Services.AddHostedService<QueuedHostedService>();
builder.Services.AddSingleton<IBackgroundTaskQueue>(_ => 
{
    if (!int.TryParse(builder.Configuration["QueueCapacity"], out var queueCapacity))
    {
        queueCapacity = 100;
    }

    return new DefaultBackgroundTaskQueue(queueCapacity);
});

IHost host = builder.Build();

MonitorLoop monitorLoop = host.Services.GetRequiredService<MonitorLoop>()!;
monitorLoop.StartMonitorLoop();

host.Run();

```

--------------------------------

### C# Partial Struct and Interface Examples

Source: https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/partial-classes-and-methods

Illustrates the application of the 'partial' keyword to interface and struct definitions in C#. This example shows how methods can be declared across multiple partial definitions for interfaces and structs.

```csharp
partial interface ITest
{
    void Interface_Test();
}

partial interface ITest
{
    void Interface_Test2();
}

partial struct S1
{
    void Struct_Test() { }
}

partial struct S1
{
    void Struct_Test2() { }
}
```

--------------------------------

### Run .NET Projects from CLI

Source: https://learn.microsoft.com/en-us/dotnet/aspire/get-started/add-aspire-existing-app_source=recommendations

Executes .NET projects using the 'dotnet run' command. This snippet shows how to run the 'Products' and 'Store' projects individually, specifying their respective project file paths.

```dotnetcli
dotnet run --project ./Products/Products.csproj
```

```dotnetcli
dotnet run --project ./Store/Store.csproj
```

--------------------------------

### Run AppHost Project using .NET CLI

Source: https://learn.microsoft.com/en-us/dotnet/aspire/get-started/add-aspire-existing-app_source=recommendations

This command initiates the .NET Aspire AppHost project, which orchestrates the entire solution. Ensure Docker Desktop or Podman is running, as this command will start the necessary containers. After successful execution, the .NET Aspire dashboard will be accessible for monitoring and interacting with the deployed services.

```dotnetcli
dotnet run --project ./eShopLite.AppHost/eShopLite.AppHost.csproj
```

--------------------------------

### Generic Host Extension Library for .NET

Source: https://learn.microsoft.com/en-us/dotnet/standard/runtime-libraries-overview

This snippet shows how to set up a generic host using the `Microsoft.Extensions.Hosting` NuGet package. It demonstrates creating a host builder and running the host, which is a common pattern for background services and web applications in .NET.

```csharp
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

public class GenericHostExample
{
    public static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) => {
                // Configure services here
            })
            .Build();

        Console.WriteLine("Host created. Press Ctrl+C to exit.");
        await host.RunAsync();
    }
}
```
# Academic Management System CLI

A C# console application developed by **Lapko Dmytro** for managing academic subjects and their associated study sessions. This tool allows users to perform CRUD operations (Create, Read, Update, Delete) on academic data through a flexible command-line interface.

## 🚀 Features

-   **Dynamic Entity Management**: Handle both `Subject` and `Session` entities.
-   **Advanced Search**: Filter results by query strings and sort data using `ASC` (Ascending) or `DESC` (Descending) flags on any field.
-   **Reflection-Based Logic**: Automatically maps console input to object properties using C# Reflection.
-   **Python-Style Output**: Data is displayed in a clean, readable dictionary-like format.
-   **Guid Support**: Uses unique identifiers for robust data management.

---

## 🛠 Commands

The program uses a prefix-based command system. Arguments are generally space-separated, while object parameters use a `key=value` format.

| Command | Short | Description | Syntax |
| :--- | :--- | :--- | :--- |
| `help` | `h` | Show available commands | `help` |
| `search` | `s` | Search and sort entities | `s [entity] [query?] [ASC/DESC=field]` |
| `add` | `a` | Create a new record | `a [entity] [key=value] ...` |
| `update` | `u` | Update an existing record | `u [entity] [id] [key=value] ...` |
| `delete` | `d` | Remove a record by ID | `d [entity] [id]` |
| `exit` | - | Close the application | `exit` or `quit` |

---

## 📖 Usage Examples

### 1. Searching and Sorting
To view all subjects sorted by name in descending order:
```bash
#s subject DESC=Name
```

To search for a specific keyword (e.g., "Programming"):
```bash
#s subject Programming
```

### 2. Adding a New Subject
When adding, specify the properties as `Key=Value` pairs.
```bash
#a subject Name=Calculus EctsCredits=5 Area=Mathematics
```

### 3. Deleting a Session
Provide the entity type and the GUID of the record:
```bash
#d session 00fb4a06-5b74-4419-bfea-318e1e9501bc
```

### 4. Updating a Record
```bash
#u subject cf0499b6-f780-461d-ba7d-f18a5161e44d Name="Advanced C#" EctsCredits=6
```

---

## 💻 Technical Implementation Details

-   **Property Mapping**: The `FillObjectFromParameters` method uses `TypeDescriptor` and `Reflection` to convert string inputs into the correct data types (int, Guid, Enum, etc.) dynamically.
-   **Flexible Search**: The search logic checks all string-convertible properties of an object to find matches for the search query.
-   **Data Display**: The `PrintPythonStyle` method iterates through object properties to generate a structured visualization:
    ```text
    Lab1_65_Lapko.Subject: 
        'Id': 'cf0499b6-f780-461d-ba7d-f18a5161e44d'
        'Name': 'C# Programming'
        'EctsCredits': '5'
    ```

## 👤 Author
**Lapko Dmytro**
*Lab Work 1 - Academic Service Management*
Library App - Project Overview

The Library Project is a C# .NET Windows Application designed to manage library or bookstore inventory (such as books and journals). The system allows users to create, search, and manage items, adjust copy counts, and generate customized reports.

Core Features & User Interface (WpfLibrary)
•	Main Screen (MainWindow):
o	Item Creation: Users can fill out details such as title, author, price, discount, edition number, publisher, release date, and genre/category, then click "Add Item" to populate the inventory shelves.
o	Shelves & Interaction: Items are displayed on designated book and journal shelves. Double-clicking any record opens a message window with item details.
o	Copy Management: Users can add copies ("Add Book / Journal Copies") or remove copies ("Remove Book / Journal"). If the last copy is removed, the item is completely deleted from the database.
o	Search & Reports: Features a search bar for titles or unique IDs (ISBN/ISSN). The "Generate Reports" section lets users filter items by criteria (title, price range, genre/category) using a combo box and generate reports in a separate window (ReportWindow).

System Architecture & Entities (BookLib Class Library)
The solution is divided into three primary projects: BookLib (core logic), Tests (unit testing), and WpfLibrary (WPF user interface). The BookLib library contains the following key classes and interfaces:
•	ICollection: An interface enforcing core collection requirements, item lists, and addition/removal methods.
•	ItemCollection: Implements ICollection and contains the primary application logic, holding a list of AbstractItem objects and providing methods for adding, removing, filtering, indexing, and querying items.
•	AbstractItem: An abstract base class defining shared general properties for both books and journals, along with virtual methods implemented by subclasses.
•	Book & Journal: Concrete classes inheriting from AbstractItem, overriding the virtual methods SetDiscount() and GetDetails().
•	Custom Exceptions & Validation: Includes tailored exception classes (DiscountException, ItemExistsException, MissingItemException, MissingDetailesException, NoMatchException) to handle errors and ensure data integrity.

Testing & Design Patterns
•	Testing: The Tests project contains UnitTest1 to validate the components and functions of the core BookLib library.
•	MVVM Option: The project supports an optional architectural pattern using Model-View-ViewModel to separate business logic from the graphical user interface.


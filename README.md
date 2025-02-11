# Accubid Connector for App Xchange

## Overview
The **Accubid Connector for App Xchange** enables seamless integration between Trimble Accubid Anywhere and other enterprise applications via Trimble App Xchange. This connector facilitates data exchange between Accubid's estimating, project management, and closeout modules, ensuring that key information flows efficiently across systems.

### Key Features:
- Secure authentication via **OAuth 2.0 Code Flow**
- Structured data access through well-defined **modules** and **data objects**
- Efficient handling of estimates, projects, contracts, and change orders
- API-driven approach to streamline workflows in construction estimating

For more details, refer to the official **[Accubid Anywhere Developer Documentation](https://developer.trimble.com/docs/accubid-anywhere)**.

---

## Project Structure
The connector is organized into multiple modules, each representing a key functional area in Accubid. Each module contains **data objects**, which define the specific entities that can be retrieved or modified.

### **Modules and Data Objects**
#### **1. Database Module** (`database-1`)
- **Databases**: Retrieves a list of available databases within Accubid Anywhere.

#### **2. Estimate Module** (`estimate-1`)
- **Estimate**: Represents a single estimate, including cost breakdowns and components.
- **Estimates**: Retrieves multiple estimates from the system.
- **EstimatesByDueDate**: Retrieves estimates based on their due date.
- **ExtensionItemDetailsFileSignalR**: Handles real-time extension item details via SignalR.
- **NotificationTest**: Used for testing notification-based updates.

#### **3. Project Module** (`project-1`)
- **LastProjects**: Fetches recently accessed projects.
- **Project**: Represents a single project, including associated metadata and estimates.
- **Projects**: Retrieves multiple projects within Accubid Anywhere.

#### **4. Closeout Module** (`closeout-1`)
- **FinalPrice**: Provides the final contract price after closeout.

#### **5. Change Order Module** (`changeorder-1`)
- **ContractCostDistribution**: Breaks down contract costs across different categories.
- **ContractQuoteLabels**: Retrieves labels associated with contract quotes.
- **Contracts**: Represents a list of contracts in Accubid.
- **ContractStatuses**: Retrieves the status of different contracts.
- **ContractSubcontractLabels**: Fetches labels related to subcontractors within contracts.
- **PCO**: Represents a single Potential Change Order (PCO).
- **PCOs**: Retrieves multiple Potential Change Orders.

---

## Authentication
This connector utilizes **OAuth 2.0 Code Flow** to authenticate API requests securely. The authentication setup ensures that data is accessed only by authorized users and applications.

To establish authentication:
1. Register your application with Accubid Anywhere.
2. Obtain client credentials (Client ID and Client Secret).
3. Implement the OAuth 2.0 Code Flow to obtain an access token.
4. Use the access token to authenticate API requests.

---

## Use Cases
The Accubid Connector enables various automation and integration use cases, including:
- **Estimating Workflow Automation**: Syncing estimates from Accubid to ERP systems for financial planning.
- **Project Data Integration**: Seamless transfer of project details between Accubid and other project management platforms.
- **Contract & Change Order Management**: Keeping contract statuses and potential change orders updated in real-time.
- **Real-Time Notifications**: Using SignalR to push live updates on estimate changes.

---

## Additional Resources
For further details, visit:
- [Trimble App Xchange Documentation](https://developer.trimble.com/docs/app-xchange)
- [Accubid Anywhere API Reference](https://developer.trimble.com/docs/accubid-anywhere)

For support and troubleshooting, contact **Trimble Developer Support**.


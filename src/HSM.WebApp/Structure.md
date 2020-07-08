Following features are needed in the system,
    *   Member mangement
            -   CRUD operations
            -   Login
    *   Management System
            -   Login
            -   Memebers rights (President, GS etc.)
    *   Maintaince Collection System
            -   Initial deposit
            -   Monthly/ Weekly dues
    *   Accounts
            -   Cash
            -   Bank
            -   Expenses
            -   Collection
            -   Expense-Due
            -   Collection-Due
    *   Grivance mangement
            -   Create operation
            -   Update in statges
            -   Resolved at the end
    *   Voting on issue
            -   Topic (created by management)
            -   Voting by members
            -   Result - (by ownership area/ by person count)

Migration commands,
- dotnet ef migrations add InitialCreate
- dotnet ef database update
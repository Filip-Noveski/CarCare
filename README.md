# CarCare
**CarCare** is a desktop application intended to allow enthusiasts to track maintanance and running costs related to their cars, as well as a gallery of images from their faviourite moments with each machine. The app stores the data locally on your PC, so there's no need for communication with third-pary servers or external accounts. 
*All data is stored **locally** on your PC — no third-party servers or external accounts required.*

# Features
## Included
* User Accounts for personalised settings, and private data storage and access.

## Planned
* Costs tracking for multiple cars per account.
* Financial breakdown of costs over time.
* Image upload for gallery.
* Import/Export of data for switching between PCs.

# Technologies
The **CarCare** application is built on the **.NET Core** framework, utilising **Windows Presentation Foundation (WPF)** for the user-interface.
Data is persisted in an **SQLite** database, accessed via **Dapper**.
All services are unit tested with **xUnit** paired with **NSubstitute** and **Fluent Assertions**.

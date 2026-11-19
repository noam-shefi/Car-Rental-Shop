# Car Rental Domain Conversion Audit

## Scope and status

Read-only audit completed before renaming. Covered the active solution projects, authored Web/WPF/WCF/model/data-access source, project and solution identifiers, configuration, documentation, and Access database files. Vendored `packages`, generated `PrecompiledWeb`, build outputs, and generated service artifacts are not independent source-of-truth names; they must be rebuilt or regenerated after authoritative contract changes.

Implementation has begun but the domain and database conversion is **not complete**. The shared web theme was renamed and its master-page references updated. Retained cart markup now identifies the legacy cart without describing its items as hardware; it is not represented as a rental booking. A pre-existing working-tree deletion at `ViewModelHardwareStore/App_Data/HardwareStore.laccdb` remains untouched.

### Implementation checkpoint and migration blockers

- Visual Studio reported that the application was being debugged, and IIS Express processes were running. An exclusive open of the active database succeeded momentarily, but this does not freeze writes or establish that a snapshot remains current. No database was copied, migrated, renamed, or switched; neither original is a backup of the other. Stop all writers and retain an exclusive lock throughout a consistent snapshot, then compare against the original before any cutover.
- Read-only aggregate queries on the active database found 18 products, all currently marked as cars; 10 orders include 6 rentals, leaving 4 legacy orders. Cart and order-details tables each had 0 rows at inspection time. These counts are a point-in-time observation, not permission to discard legacy workflows or rows.
- `Product.Price` and `StockQuantity` are shared with retail cart/order operations. For cars, price is multiplied by rental days and quantity is fleet capacity against overlapping reservations. `Category` is a shared legacy classification while `CarCategory` stores the vehicle class; neither can safely be blindly renamed into the other. Shipping addresses and mixed orders likewise require a data-preserving mapping.
- Initial and final SHA-256 hashes were unchanged: active `ClassLibrary2/App_Data/HardwareStore.accdb`: `5EC2DCAA5B6572E620F4DB8F57408ED991E11FC3BAF8796D0E5CE22E215A1EBF`; separate legacy `ViewModelHardwareStore/App_Data/HardwareStore.accdb`: `9788BDD2CB1E374349BE030C1AEDCACEF84A805EECF549FBA99347309C3E8F8B`. No backup was created in this checkpoint.
- Runtime repair is verified on disk: the existing IIS Express service site now points to `WcfServiceCarRental`; the website references the renamed model assembly; the stale website-only `ClassLibrary1.dll` output was removed; and `Web.config` directs the site root to `HomeVisitor.aspx`. The full `HardwareStore.sln` MSBuild succeeds with five ASP.NET compiler warnings (CS1685/CS1702). The website root, car catalogue, recommendation page, WCF metadata, and payment test endpoint all returned HTTP 200. Recommendation training remains disabled and its read-only database query returned three dummy rows. The active and legacy database hashes are unchanged; no database was migrated or switched.
- Visual Studio's current in-memory diagnostics still reference excluded legacy `ProductDB.cs`/`BrandDB.cs` files and report an edit-and-continue attribute warning. Reload the existing solution (or close and reopen `HardwareStore.sln`) and stop the old debug session before F5; do not save the stale in-memory solution.
- Next: quiesce writers, create and verify independent backups, migrate a copy with explicit mappings, validate schema/indexes/data/relationships and rental operations, then coordinate model, service and client contract changes before switching runtime. Keep the existing description/specification grouping on car pages. Stages 1–4 and the remaining portions of stages 5–7 are outstanding; do not treat the theme rename as a full conversion.

## Terminology checklist

| Done | Existing name/concept | Proposed rental terminology | Representative authored locations / notes |
|---|---|---|---|
| [ ] | `HardwareStore`, `BusinessModelHardwareStore`, `ViewModelHardwareStore`, `WcfServiceHardwareStore` | `CarRental`, `BusinessModelCarRental`, `ViewModelCarRental`, `WcfServiceCarRental` | `HardwareStore.sln`; the corresponding project folders/files, assembly names, namespaces and references. Preserve the existing project/layer structure. |
| [ ] | `Product`, `ProductDB`, `ProductID`, `ProductName`, `MyProduct*` | `Car` or `FleetVehicle`, `CarDB`, `CarID`, `ModelName`, `MyCar*` | Domain/model, data-access, WCF contracts and clients, shopping/cart/order/review/favorite references, WPF `ProductWindow`, Web `ProductDetailsUser`. Rename server and client contracts together. |
| [ ] | `Brand`, `BrandDB`, `BrandID`, `MyBrand*` | `Make` or `Manufacturer`, consistently | Domain/model, DB queries, WCF, `BrandWindow`. Confirm whether this represents the vehicle make or fleet operator before choosing. |
| [ ] | `Category`, `CategoryDB`, generic category IDs | `VehicleCategory` / `VehicleClass` | Domain/model, DB, WCF and `CategoryWindow`; retain current rental values such as `sedan`, `suv`, `sports`, and `family`. |
| [ ] | `StockQuantity`, `ManageStock`, `SetCarStock` | `FleetQuantity` or `AvailableUnits`, `ManageFleet`/`ManageAvailability`, `SetCarAvailability` | Data/model, admin pages and service client. Verify whether quantity means fleet capacity or bookable availability before renaming. |
| [ ] | `Cart`, `CartDB`, `CartUser`, `CartWindow`, “Shopping Cart” | `Reservation`/`Booking` equivalents | Existing cart data is product/quantity based; convert only after preserving its data and behavior. See `BusinessModelHardwareStore/Cart.cs`, `ClassLibrary2/CartDB.cs`, `Web/CartUser.aspx`, `WpfApp1/CartWindow.xaml`. |
| [ ] | `CheckoutUser`, retail checkout labels | `ReservationCheckout` / `BookingCheckout` | `Web/CheckoutUser.aspx*`; ensure this is a rental booking workflow, not just a label change. |
| [ ] | `Order`, `OrderDetails`, `OrderID`, “Order History” | `RentalReservation`, `ReservationLine`, `ReservationID`, “Rental History” for rental records | `Order` currently mixes legacy retail and rental records (`IsRental`). Maintain IDs/relationships and distinguish legacy rows during migration. |
| [ ] | `ProductID` links in Cart, OrderDetails, Review, Favorite | `CarID` | Update all foreign references and WCF serialization in coordination with the schema. |
| [ ] | `ShippingAddress`, `ShippingAddressDB`, `ShippingAddressWindow` | `PickupLocation`/`ReturnLocation` or `RentalLocation` | Do not blindly map one shipping address to both pickup and return; current booking requirements need review. |
| [ ] | `ForgePCUser`, `PCPredictor`, `BuildMyPC`, `ShopUser` | Remove obsolete PC-builder concepts or replace with specific rental recommendation/browse names | `Web/ForgePCUser.aspx*`, `Web/App_Code/PCPredictor.cs`, WPF datasource; confirm feature ownership before removal. |
| [ ] | User-facing “hardware”, “forge”, “Product”, “Product Image”, “Product Name”, “Go Shopping” | Fleet/rental wording such as “Car”, “Car image”, “Model”, “Browse cars” | `Web/CartUser.aspx`, `Web/AccountUser.aspx`, WPF admin windows and other Web labels/messages/titles. |
| [x] | `hardwarestore-theme.css` | `carrental-theme.css` | Renamed in `Web/css` and both authored master-page references updated. Generated precompiled copies were not edited. |
| [ ] | `HardwareStore.accdb`, startup/setup text | `CarRental.accdb`, rental-oriented setup documentation | `ClassLibrary2/DBfunction.cs`, project content declarations, `STARTUP.md`, runtime copy/configuration paths. Only after migration and source selection. |
| [ ] | Comments/docstrings using hardware/product/purchase terminology | Accurate rental-domain wording | WCF service/interface and DB/model comments, documentation and messages. |
| [ ] | WCF/service metadata and connected-service generated names | Regenerate/update contracts from authoritative service after source changes | Keep service/client operation and serialization contracts synchronized; generated files are not to be edited independently. |

### Names already consistent with the rental domain

Preserve/refine rather than mechanically rename: `Cars`, `CarDetails`, `CarEdit`, `CarMatch`, `FleetRecommendations`, `ManageBranches`, `CarRentalServiceClient`, `GetCars`, `SearchCars`, `AddCar`, `UpdateCar`, `CreateRentalOrder`, `RentalStartDate`, `RentalEndDate`, `IsRental`, `RentalStatus`, and `CancellationRequested`.

### Non-domain names / exclusions

Do not change framework/runtime terms or third-party package names. The `visa` application, `VisaDB.accdb`, generic `User`, `Contact`, `Review`, and service plumbing need not be renamed absent a domain-specific reason. Generated `bin`, `obj`, and `PrecompiledWeb` content should be regenerated, not treated as authored identifiers.

## Access database audit

The primary data layer uses Microsoft Access `.accdb` through ACE OLE DB (`ClassLibrary2/DBfunction.cs`), not SQL Server. There is no complete authored SQL schema script. The application adds some rental fields/tables automatically when the DB layer initializes. The source also contains an absolute developer-machine database path and copies database files into runtime outputs.

### Current rental-capable database: `ClassLibrary2/App_Data/HardwareStore.accdb`

Tables, columns and keys found by read-only schema inspection:

| Table | Columns (PK/indexes noted) |
|---|---|
| `Branch` | `BranchID` (PK), `BranchName`, `City`, `Address`, `Phone` |
| `Brand` | `BrandID` (PK), `BrandName`, `CountryOfOrigin`, `WebsiteURL`, `Description` |
| `Cart` | `ProductID`, `Username`, `Price`, `Quantity`; composite PK `(ProductID, Username)` |
| `Category` | `CategoryID` (PK), `CategoryName`, `Description` |
| `Contact` | `ContactID` (PK), `Username`, `FullName`, `Email`, `Subject`, `Message`, `DateSent`, `Treated` |
| `Favorite` | `Username`, `ProductID`, `DateAdded`; composite PK `(Username, ProductID)` |
| `Order` | `OrderID` (PK), `Username`, `OrderDate`, `TotalPrice`, `Address`, `AdditionalInstructions`, `Delivered`, `IsRental`, `CarID`, `RentalStartDate`, `RentalEndDate`, `RentalStatus`, `CancellationRequested` |
| `OrderDetails` | `OrderDetailsID` (PK), `OrderID`, `ProductID`, `Quantity`, `UnitPrice` |
| `Product` | `ProductID` (PK), `ProductName`, `CategoryID`, `BrandID`, `Price`, `StockQuantity`, `Description`, `Picture`, `IsCar`, `CarYear`, `CarCategory`, `BranchID`, `Seats`, `FuelType`, `RangeKm`, `Transmission`, `LuggageCapacity` |
| `Review` | `ReviewID` (PK), `Username`, `ProductID`, `Rev`, `Rating`, `RentalOrderID`, `ReviewDate`; unique index on `RentalOrderID` |
| `ShippingAddress` | `Username` (PK), address fields |
| `User` | `Username` (PK), `Password`, `PhoneNumber`, `Email`, `Admin`, `JoinDate`, `FirstName`, `LastName` |

No enforced foreign-key constraints were exposed by the schema inspection; relationships are currently maintained by application logic/queries. The initializer in `ClassLibrary2/DBfunction.cs` creates `Branch`, `Favorite`, rental columns and a review index.

### Other database files

- `ClassLibrary2/App_Data/HardwareStore.before-car-import-20261114-181855-c8050076b1b74d1887df6cdf2fb391a4.accdb`: same table set as the primary current DB.
- `ViewModelHardwareStore/App_Data/HardwareStore.accdb`: distinct legacy schema (`Brand`, `Categorie`, `Customer`, `Order`, `OrderDetail`, `Product`); it is not interchangeable with the current DB.
- `Web/App_Data/RecommendationTraining.accdb`: `RecommendationGroups`.
- `visa/App_Data/VisaDB.accdb`: `Visa`; a precompiled runtime duplicate also exists.
- Database copies also occur under project `bin/App_Data` directories. `ClassLibrary2/ViewModelHardwareStore.csproj` copies one DB to output on every build, creating a stale-copy risk.

### Database objects and mappings

| Existing table/column | Proposed rental name | Migration note |
|---|---|---|
| `Product` / `ProductID` | `Car` or `FleetVehicle` / `CarID` or `VehicleID` | Preserve ID mapping and all cross-table references. |
| `ProductName` | `ModelName` / `CarName` | Keep descriptive text separate. |
| `Brand` / `BrandID` | `Make` or `Manufacturer` / `MakeID` | Decide whether this is vehicle make or fleet operator. |
| `Category` / `CategoryID` | `VehicleCategory` / `VehicleCategoryID` | Keep legacy data and rental category values. |
| `Price` | `DailyRate` | Existing rental UI treats car price as a daily rate; verify all consumers. |
| `StockQuantity` | `FleetQuantity` / `AvailableUnits` | Confirm capacity vs current availability semantics before conversion. |
| `Order` rental rows | `RentalReservation` / `ReservationID` | Separate `IsRental=True` records from legacy retail-shaped rows; preserve IDs and `OrderDetails`/review links. |
| `OrderDetails` / `ProductID` | `ReservationLine` / `CarID` | Preserve order relationship and quantities/prices; determine whether quantity makes sense for a reservation. |
| `RentalStartDate`, `RentalEndDate` | `PickupDate`, `ReturnDate` | Preserve existing values and reservation semantics. |
| `Branch` / `BranchID` | `RentalBranch` / `BranchID` or `LocationID` | Preserve assignments and branch lookup relationships. |
| `Favorite` / `ProductID` | `FavoriteCar` / `CarID` | Preserve composite key and dates. |
| `Review.RentalOrderID` | `Review.ReservationID` | Preserve unique index and all review references. |
| `ShippingAddress` | Pickup/return location entities if required | Requires explicit workflow/data mapping; not a safe one-to-one rename. |

## Required safe migration plan (before schema or runtime DB changes)

1. Freeze writes and identify the one authoritative runtime database/path; resolve the hard-coded absolute path and duplicated output copies first.
2. Take immutable backups of every relevant `.accdb`, verify each backup opens read-only, and record file hashes. Never rename, overwrite, or delete an original database as the migration step.
3. Export actual schema, indexes, constraints, and row counts; compare the current rental-capable database with the distinct legacy `ViewModelHardwareStore` schema.
4. Approve an explicit field/table mapping, especially `Brand` semantics, inventory semantics, `ShippingAddress`, and mixed `Order.IsRental` records. Create ID crosswalks if keys cannot be preserved directly.
5. Migrate into a new database file and leave old tables/data intact until validation is accepted. Ensure favorites, cart, reservations, rental dates/status, branches, reviews, users, and legacy rows remain linked correctly.
6. Run validation queries/checks: table counts and key uniqueness; all relationship lookups; unique rental review constraint; rental overlap/availability calculations; cancellation/status transitions; WCF service reads/writes; no orphan references.
7. Only after successful validation, update DB configuration and project content-copy settings; disable uncontrolled automatic schema mutation and make runtime DB selection explicit. Keep a rollback path to the untouched originals.
8. Rebuild all solution projects, regenerate WCF metadata/clients, run relevant application/tests, and verify deployment/runtime opens the intended migrated database.

**Important:** The safe migration must precede changing database filenames, connection paths, table/column references, or output-copy configuration. A simple file rename is not a schema/data migration. The inspected databases have different schemas, and the current initializer can mutate whichever DB it opens.

## Observed workspace state

At audit time, `git status --short` showed a pre-existing deletion: `ViewModelHardwareStore/App_Data/HardwareStore.laccdb`. It was not changed or restored.

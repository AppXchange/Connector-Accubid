## Accubid Connector for App Xchange

## Project Structure Commands
cd desktop/connectors
mkdir connector-accubid
cd connector-accubid
xchange connector new --name Accubid
cd connector

xchange client new --type Http --auth-type OAuth2CodeFlow

xchange module new --id database-1 --name Database --key database --version 1
xchange data-object new --module-id database-1 --name Databases

xchange module new --id estimate-1 --name Estimate --key estimate --version 1
xchange data-object new --module-id estimate-1 --name Estimate
xchange data-object new --module-id estimate-1 --name Estimates
xchange data-object new --module-id estimate-1 --name EstimatesByDueDate
xchange data-object new --module-id estimate-1 --name ExtensionItemDetailsFileSignalR
xchange data-object new --module-id estimate-1 --name NotificationTest

xchange module new --id project-1 --name Project --key project --version 1
xchange data-object new --module-id project-1 --name LastProjects
xchange data-object new --module-id project-1 --name Project
xchange data-object new --module-id project-1 --name Projects

xchange module new --id closeout-1 --name Closeout --key closeout --version 1
xchange data-object new --module-id closeout-1 --name FinalPrice

xchange module new --id changeorder-1 --name ChangeOrder --key changeorder --version 1
xchange data-object new --module-id changeorder-1 --name ContractCostDistribution
xchange data-object new --module-id changeorder-1 --name ContractQuoteLabels
xchange data-object new --module-id changeorder-1 --name Contracts
xchange data-object new --module-id changeorder-1 --name ContractStatuses
xchange data-object new --module-id changeorder-1 --name ContractSubcontractLabels
xchange data-object new --module-id changeorder-1 --name PCO
xchange data-object new --module-id changeorder-1 --name PCOs

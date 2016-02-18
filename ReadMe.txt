Rad more about this project here: http://rahulrai.in/post/hire-a-cab-using-cortana-and-azure-search
------------------
Setup Steps
------------------
1. Create DocumentDb. findmeacab.
2. Create Stream Analytics. gpssensoranalytics.
3. Create Search Service. findmeacab.
4. Create a database in DocumentDb. cabsensordata.
5. Create a collection in the DocumentDb database. cabgpsdatacollection.
6. Create EventHub. findmeacabeventhub. findmeacabeventhub-ns.
7. Connect Stream Analytics to DocumentDb. Steps: https://azure.microsoft.com/en-us/blog/azure-stream-analytics-and-documentdb-for-your-iot-application/
8. Output: PartitionKey: partitionkey | DocumentId: vehicleid
10. Input: Add data for event hub.
11. Create Bing Maps Account.
12. Create search index. cabdataindex.
13. Create search indexer through POSTMAN. documentdbindexer. Data Source: documentdbdatasource


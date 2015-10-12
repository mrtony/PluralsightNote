# CH03 - Using EF to Interact with Your Data
本章包含:
* Inserting simple object
* Query simple objects
* Updating simple object
* Get data with Find and SqlQuery
* Delete objects
* Projection Queries

* CH03-03-Inserting Objects: 使用Add(), AddRange()加資料到DB
* CH03-04-Querying Simple Objects: DB的查詢

---
# CH01 - Overview of Entity Framework 6

章節概述
* CH01-03-Creating an Entity Framework Model: 建立EF程式初步架構

---
# CH02 - Creating Code Base Model and Database
本章包含:
* 安裝EF
* 建立Domain class和Data Model

章節概述
* CH02-05-Fixing How EF Interprets Your Model: 使用EF Power tool
* CH02-06-Using Code First Migrations to Create a Database: 用`Database Migration`在SQL建立Database
* CH02-07-Migrating a Database When Your Model Changes: 加入新欄位到table時, 如何Migration Database
* CH02-08-Creating Visual and Code Models from Existing Databases: 從已建立的資料庫建立code model.

範例程式架構
* Data Model project: 放Context, 需參考EF library
* Domain project: 放domain class, 不需參考EF library

### Q&A
* 建立EF的最基本要素?<br/>
一定要繼承DbContext

* 什麼是DbSet<T>?<br/>
可以看成是一個table的集合. 一個DbSet就是一個table.

* 如何用Entity Framework Power Tool檢視Model?
   1. 安裝好Power tool
   2. 建立好domain class (要做為table中屬性的結構)
   3. 以Domain class來建立DbContext > ExampleContext.cs
   4. 在ExampleContext.cs的專案按右鍵, 選擇`Set as Startup Project`
   5. 在ExampleContext.cs按右鍵, 選擇`Entity Framework`, 再選擇`View Entity Data Model(Read-only)

* 什麼時候需要做Database Migration?<br/>
通常是在改變DB的Schema時, 會需要做DB的Migration.

* 如何用Db Migration來建立Database?<br/>
參考CH2-06, 步驟如下(在還未建立任何Database的狀態下):
   1. 使用Package Manager Console
   2. `Enable-Migrations`: 會建立`Migrations`目錄及`Cofiguration.cs`
   >輸出: Checking if the context targets an existing database...
Code First Migrations enabled for project DataModel.
   3. `Add-Migration <name-of-migration-purpose>`: 自動在`Migrations`目錄中建立`日期_name-of-migration-purpose.cs`
   >輸出: Scaffolding migration 'Initial'. The Designer Code for this migration file includes a snapshot of your current Code First model. This snapshot is used to calculate the changes to your model when you scaffold the next migration. If you make additional changes to your model that you want to include in this migration, then you can re-scaffold it by running 'Add-Migration Initial' again.
   4. `Update-Database [-Script, -Verbose]`: 套用Migration內容建立DB.
      * `Update-Database -Verbose`: 使用來直接建立DB
      * 使用`-Script`參數: **不會建立DB!!**, 會產生一個TSQL的文件, 可以用它在SQL Management Tool中執行去建立Database.
      > 輸出:Applying explicit migrations: [201510100900203_Initial].
Applying explicit migration: 201510100900203_Initial.
      * 使用`-Verbose`: 建立DB時, 會顯示詳細資訊
      
* 使用`Add-Migration <name-of-migration-purpose>`後建立的檔案中的`Up(), Down()`的用途?<br/>
   1. Up(): 執行Migration後, 會套用其內容對DB做**升級**, 也就是套新的Model內容.
   2. Down(): 執行Migration後, 會套用其內容對DB做**降級**, 也就是回復的Model內容
   
* 如何使用Migration對DB做降級或升級?<br/>
可以用`Update-Database -TargetMigration: <要升級或降的名稱>`. 其名稱就是之前用`Add-Migration <name-of-migration-purpose>`時所用的名稱.
另一個特別的用法是將DB恢復成初始狀態: `Update-Database -TargetMigration:0`, 使用時要小心.


* 加入新欄位到table時, 如何Migration Database?<br/>
參考CH2-07, 示範加入Birthday到Customer Model. 步驟如下(已建立database的狀態下):
   1. `Add-Migration AddBirthdayToCustomer`: 自動在`Migrations`目錄中建立`日期_AddBirthdayToCustomer.cs`
   2. `Update-Database -Verbose`: 直接執行升級
完成後在DB中會在增加Customer Model中新增Birthday欄位.

* 如何從現有的Database建立Code Model?<br/> 
參考CH2-08, 步驟如下(已建立database的狀態下):
   1. 建立一個class library, 並新增ADO.NET Entity Data Model, 並選擇`EF Designer from database`
   2. 選擇要加入的table/view/store_procedure, 完成後會產生example.edmx
   3. example.edmx會包含2個tt: example.context.tt, example.tt
      * example.context.tt: 繼承DbContext, 也就是DataModel.
      * example.tt: 資料Model, 也就是Domain Class, 用來定義Table的屬性及關聯

* 如何從現有的Database建立Code First?<br/>
參考CH2-08, 步驟如下(已建立database的狀態下):
   1. 建立一個class library, 並新增ADO.NET Entity Data Model,並選擇`Code first from database`
   2. 選擇要加入的table/view, 完成後會產生Domain class及context的class.
 
* 如何使用EF加入資料到DB?
參考CH2-08, 步驟如下(已建立database的狀態下):
   1. 建立Domain.class, DataModel(context) class
   2. 建立console application
   3. 建立Model實體, 並使用`Add()`來加入db, 最後再呼叫`SaveChanges()`,
   因為Order關聯到Customer(1 to many), 所以要指定CustomerId=1.
   ```
   private static void InsertOrder()
{
    var order = new Order
    {
        Cost = 100,
        CustomerId = 1
    };

    using (var db = new SaleContext())
    {
        db.Orders.Add(order);
        db.SaveChanges();
    }
}
   ``` 
   4. 如果要加入多筆(使用EF內建方法: AddRange()或可使用EF_BulkInsert套件)
   ```
private static void InsertOrders()
{
    var orders = new List<Order>
    {
        new Order { CustomerId = 1, Cost = 200},
        new Order { CustomerId = 1, Cost = 300}
    };

    using (var db = new SaleContext())
    {
        db.Orders.AddRange(orders);
        db.SaveChanges();
    }
}
   ```

### Reference
* Entity Framework: `Install-Package EntityFramework`
* [Database Answer-提供很多Schema設計參考](http://www.databaseanswers.org/data_models/)

### Tools
* [Entity Framework power tool](https://visualstudiogallery.msdn.microsoft.com/72a60b14-1581-4b9b-89f2-846072eff19d) 
* [Installing EF Power Tools into VS2015](http://thedatafarm.com/data-access/installing-ef-power-tools-into-vs2015/)
* [EntityFramework.BulkInsert - 取代EF內建的Insert,效能快約20倍](https://efbulkinsert.codeplex.com/)



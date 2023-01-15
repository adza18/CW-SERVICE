using ServiceCenterManagement.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ServiceCenterManagement.Data
{

    //A service that contains methods for the addition,deletion,updation, retrieval of records to and from record json file
    public class RecordService
    {
        public static string recordFilePath = UtilService.getRecordFilePath();
        public static string directoryPath = UtilService.getAppDirectoryPath();
        public static List<RecordModel> GetRecords()
        {
          
            if (!File.Exists(recordFilePath))
            {
                return new List<RecordModel>();
            }
            var jsonData = File.ReadAllText(recordFilePath);
            if (jsonData.Length == 0)
            {
                return new List<RecordModel>();

            }
            else
            {
                return JsonSerializer.Deserialize<List<RecordModel>>(jsonData);
            }

        }

        public static void SaveAll(List<RecordModel> records)
        {
   
            if(!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            var jsonData = JsonSerializer.Serialize(records);
            File.WriteAllText(recordFilePath,jsonData);

        }

        public static RecordModel GetRecordById(Guid id)
        {
            var records = GetRecords();
            return records.FirstOrDefault(x => x.Id == id);

        }

        //Called when admin updates an order either reject or approve
        public static List<RecordModel> UpdateRecord(Guid recordId, Guid userId, OrderStatus status)
        {
            var orderedDay = (int)DateTime.Now.DayOfWeek;
            var orderedTime = DateTime.Now.Hour;
            if (orderedDay < 1 || orderedDay > 6)
            {
                throw new Exception("Order can be updated only between Monday and Friday");

            }
            if (orderedTime < 9 || orderedTime > 18)
            {
                throw new Exception("Order can be updated only between 9AM and 6PM ");

            }
            var records = GetRecords();
            var items = ItemService.GetItems();
            var record = records.FirstOrDefault(x => x.Id == recordId);
            var item = items.FirstOrDefault(x => x.Id == record.ItemId);
            if(record == null)
            {
                throw new Exception("No such record");
            }
            if (status == OrderStatus.Approved)
            {
                record.ApprovedBy = userId;
                record.IsApproved = true;
                record.Status = OrderStatus.Approved;

                record.ApprovedDate = DateTime.Now;
                item.Stock -= record.QuantityTakenOut;
            }
            else if(status == OrderStatus.Rejected)
            {
                record.IsApproved = false;
                record.Status = OrderStatus.Rejected;


            }
            ItemService.SaveAll(items);
            SaveAll(records);
            return records;
        }

        //Called when a staff places an order
        public static List<RecordModel> RecordLog(Guid userId, Guid productId, int qty)
        {
           
            var records = GetRecords();
            var items = ItemService.GetItems();
            var item = items.FirstOrDefault(x => x.Id == productId);
            if (qty > item.Stock)
            {
                throw new Exception("Not enough stock");
            }
            if (qty <= 0)
            {
                throw new Exception("Order quantity must be greater than zero.");

            }
            records.Add(new RecordModel
            {
                RequestedBy = userId,
                ItemId = productId,
                QuantityTakenOut = qty,
                Status = OrderStatus.Pending,
                OrderTotal = item.Price * qty,
            });
            item.LastOrderedDate = DateTime.Now;
            ItemService.SaveAll(items);
            SaveAll(records);
            return records;

        }

        //Called to get item and quantity relationship for bar graph
        public static List<RecordStatistic> GetItemQtyRelation()
        {
            var records = GetRecords().Where(x => x.IsApproved);
            var groupedRecord = records.GroupBy(x => x.ItemId).ToList();
           
            var result = (from record in records
                          group record by record.ItemId
                          into item
                          select new RecordStatistic
                          {
                              ItemId = item.Key,
                              ItemQuantity = item.Sum(x => x.QuantityTakenOut),
                          }).ToList();

            

           return result;

        }

    }
    }


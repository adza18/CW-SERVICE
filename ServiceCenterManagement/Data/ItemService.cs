using ServiceCenterManagement.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
namespace ServiceCenterManagement.Data
{
    //A service that contains methods for the addition,deletion,updation, retrieval of items to and from item json file, and for merge sort
    public class ItemService
    {
        public static string itemFilePath = UtilService.getItemFilePath();

        public static string directoryPath = UtilService.getAppDirectoryPath();
        public static List<ItemModel> GetItems()
        {
            
            if (!File.Exists(itemFilePath))
            {
                return new List<ItemModel>();
            }
           
            var jsonData = File.ReadAllText(itemFilePath);

            if (jsonData.Length == 0)
            {
                return new List<ItemModel>();

            }
            else
            {
                return JsonSerializer.Deserialize<List<ItemModel>>(jsonData);
            }
         


		}

		public static ItemModel GetByItemId(Guid itemId)
        {
           
            var items = GetItems();
            return items.FirstOrDefault(x => x.Id == itemId);
        }


        public static List<ItemModel> AddItem(Guid itemId, string name, int stock, string desc,int price)
        {
            var items = GetItems();

            if(string.IsNullOrEmpty(name))
            {
                throw new Exception($"Item name cannot be empty");
            }

            if (stock <= 0)
            {
                throw new Exception($"Stock should be at least one");

            }

            if(price <=0)
            {
				throw new Exception($"Price should be greater than zero");

			}
		

            var item = new ItemModel()
            {
                Name = name,
                Stock = stock,
                Desc = desc,
                Price = price,
                AddedBy = Guid.NewGuid(),
                AddedDate = DateTime.Now,
                LastOrderedDate = DateTime.Now,
            };
            items.Add(item);
            SaveAll(items);
            return items;

        }
        public static List<ItemModel> MergeSort(List<ItemModel> unsorted)
        {
            if (unsorted.Count <= 1)
                return unsorted;

            var left = new List<ItemModel>();
            var right = new List<ItemModel>();

            int middle = unsorted.Count / 2;

            for (int i = 0; i < middle; i++)
            {
                left.Add(unsorted[i]);
            }

            for (int i = middle; i < unsorted.Count; i++)
            {
                right.Add(unsorted[i]);
            }

            left = MergeSort(left);
            right = MergeSort(right);
            return Merge(left, right);
        }

        public static List<ItemModel> Merge(List<ItemModel> left, List<ItemModel> right)
        {
            var result = new List<ItemModel>();

            while (left.Count > 0 || right.Count > 0)
            {
                if (left.Count > 0 && right.Count > 0)
                {
                    if (left.First().Stock <= right.First().Stock)
                    {
                        result.Add(left.First());
                        left.Remove(left.First());
                    }
                    else
                    {
                        result.Add(right.First());
                        right.Remove(right.First());
                    }
                }
                else if (left.Count > 0)
                {
                    result.Add(left.First());
                    left.Remove(left.First());
                }
                else if (right.Count > 0)
                {
                    result.Add(right.First());

                    right.Remove(right.First());
                }
            }
            return result;
        }

        


        public static List<ItemModel> UpdateItem(Guid itemId, string name, int stock, int price, Guid editedBy)
        {
            var items = GetItems();
            var item = items.FirstOrDefault(x => x.Id == itemId);
            if(item == null)
            {
                throw new Exception("Item not found");
            }
            item.Name= name;
            item.Stock= stock;
            item.Price= price;
            item.AddedBy= editedBy;
            item.LastOrderedDate = DateTime.Now;
            SaveAll(items);
            return items;
        }
        public static void SaveAll(List<ItemModel> items)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            var jsonData = JsonSerializer.Serialize(items);
            File.WriteAllText(itemFilePath, jsonData);
        }
        public static List<ItemModel> Delete(Guid id)
        {
            var items = GetItems();
            var item = items.FirstOrDefault(x => x.Id == id);
            if (item == null)
            {
                throw new Exception("Item doesnt exists");
            }
            items.Remove(item);
            SaveAll(items);
            return items;
        }
    }
}

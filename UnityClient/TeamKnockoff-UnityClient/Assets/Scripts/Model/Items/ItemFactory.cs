using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Assets.Scripts.Utilities.ExtensionMethods;
using UnityEngine;

namespace Assets.Scripts.Model.Items {
    public class ItemFactory : MonoBehaviour{
        // TODO: Sebastian
        // Implement the ItemFactory
        // Use SkillFactory for reference

        // Make sure ItemFactory follows Singleton Pattern
        // As does the Skill Factory

        // When done, add the ItemFactory.cs script to the ClientObject
        // as a component and OVERRIDE all prefabs

        public static ItemFactory instance;

        public HashSet<Item> ItemBank { get; private set; }

        private Dictionary<string, Func<Item>> itemGenerator;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(this.gameObject);
            List<Item> items = new List<Item>();

            foreach (Type type in
                Assembly.GetAssembly(typeof(Item)).GetTypes()
                    .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Item))))
            {
                items.Add((Item) Activator.CreateInstance(type));
            }

            ItemBank = new HashSet<Item>();
            ItemBank.AddRange(items);

            itemGenerator = new Dictionary<string, Func<Item>>();
            foreach (var item in ItemBank)
            {
                itemGenerator.Add(item.ItemName, item.Generate);
            }
        }

        public void Start()
        {
            
        }

        public Item GenerateItem(string name)
        {
            return itemGenerator[name].Invoke();
        }
    }
}

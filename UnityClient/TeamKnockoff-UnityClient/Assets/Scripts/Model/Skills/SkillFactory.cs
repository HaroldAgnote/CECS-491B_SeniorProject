using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Assets.Scripts.Utilities.ExtensionMethods;
using UnityEngine;

namespace Assets.Scripts.Model.Skills {
    public class SkillFactory : MonoBehaviour {
        public static SkillFactory instance;

        public HashSet<Skill> SkillBank { get; private set; }

        private Dictionary<string, Func<Skill>> skillGenerator;

        void Awake() {
            //Check if instance already exists
            if (instance == null) {
                //if not, set instance to this
                instance = this;
            }

            //If instance already exists and it's not this:
            else if (instance != this) {
                //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
                Destroy(gameObject);
            }

            DontDestroyOnLoad(this.gameObject);
            List<Skill> skills = new List<Skill>();
            foreach (Type type in
                Assembly.GetAssembly(typeof(Skill)).GetTypes()
                    .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Skill)))) {
                skills.Add((Skill) Activator.CreateInstance(type));
            }

            //initalize SkillBank with all skills
            SkillBank = new HashSet<Skill>();
            SkillBank.AddRange(skills);

            skillGenerator = new Dictionary<string, Func<Skill>>();
            foreach (var skill in SkillBank) {
                skillGenerator.Add(skill.SkillName, skill.Generate);
            }
        }

        public void Start() {
        }

        public Skill GenerateSkill(string name) {
            return skillGenerator[name].Invoke();
        }
    }
}

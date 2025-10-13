/*
	Author: Jan Holy
	Date: 12. 10. 2025
*/
using System;
using System.Collections.Generic;

namespace BattleSim
{
	internal class Program {
		static void Main(string[] args) {
			Wizard Oz = new Wizard("OZ", 5, 2);
			Warrior Bob = new Warrior("Bob", 7, 2);
			Bob.Attack(Oz);
			Console.WriteLine(Bob);
			Console.WriteLine(Oz);
		}
	}
	
	public class Entity {
		public Entity(string name, int health, int power) {
			Name = name;
			Health = health;
			Power = power;
		}
		protected string Name;
		protected int Health;
		protected int Power;

		public virtual void Attack(Entity target) { }

		public virtual void TakeDamage(int amount, Entity attacker) {
			Health -= amount;
		}

		public bool IsAlive() {
			if (Health > 0) {
				return true;
			} else {
				return false;
			}
		}

		public override string ToString() {
			return $"{{{this.GetType()}}} {{{Name}}} {{{Health}}}/{{{Power}}}";
		}
	}

	//// DEFINOVANI POSTAV/ENTIT
	public class Wizard : Entity {
		public Wizard(string name, int health, int power) : base(name, health, power) {

		}
		
		public override void Attack(Entity target) {
			Console.WriteLine("YOU SHALL NOT PASS!");
			target.TakeDamage(Power, this);
		}
		public override void TakeDamage(int amount, Entity attacker) {
			Health -= amount;
			attacker.TakeDamage(amount/2, this);
			
		}
	}

	public class Warrior : Entity {
		public Warrior(string name, int health, int power) : base(name, health, power) {

		}

		public override void Attack(Entity target) {
			Console.WriteLine("FOR GLORY!");
			target.TakeDamage(Power, this);
		}
	}

	public class Archer : Entity {
		public Archer(string name, int health, int power) : base(name, health, power) {

		}

		public override void Attack(Entity target) {
			Console.WriteLine("AND YOU HAVE MY BOW!");
			target.TakeDamage(Power, this);
		}
	}
}


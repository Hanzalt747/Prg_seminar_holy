
# 🧱 Základní pilíře OOP v C#

V C# (a obecně v objektově orientovaném programování) rozlišujeme **čtyři základní pilíře OOP**:

---

## 1. 🧩 Zapouzdření (Encapsulation)

### 💡 Myšlenka
Třída **skrývá svůj vnitřní stav** a poskytuje přístup jen prostřednictvím veřejných metod nebo vlastností.  
To chrání data před nechtěnými zásahy a udržuje konzistenci objektu.

### 💻 Příklad
```csharp
public class BankAccount
{
    private decimal balance; // skrytý stav

    public void Deposit(decimal amount)
    {
        if (amount > 0)
            balance += amount;
    }

    public decimal Balance
    {
        get { return balance; }  // přístup přes getter
        private set { balance = value; } // chráněný setter
    }
}
```

✅ Třída `BankAccount` nedovolí zvenku měnit `balance` přímo.  
Všechno musí jít přes kontrolované metody nebo vlastnosti.

---

## 2. 🧩 Dědičnost (Inheritance)

### 💡 Myšlenka
Jedna třída **může dědit vlastnosti a chování** jiné třídy.  
Díky tomu můžeš **znovu použít kód** a vytvářet hierarchie tříd.

### 💻 Příklad
```csharp
public class Animal
{
    protected string Name; // chráněný přístup – dědic má přístup, ale vnější svět ne

    public void Eat()
    {
        Console.WriteLine($"{Name} jí.");
    }
}

public class Dog : Animal
{
    public Dog(string name)
    {
        Name = name;
    }

    public void Bark()
    {
        Console.WriteLine($"{Name} říká: Haf!");
    }
}

class Program
{
    static void Main()
    {
        Dog dog = new Dog("Rex");
        dog.Eat();  // zděděno z Animal
        dog.Bark(); // vlastní metoda
    }
}
```

✅ `Dog` dědí metodu `Eat()` z `Animal` a má přístup k chráněné proměnné `Name`.

---

## 3. 🧩 Polymorfismus (Polymorphism)

### 💡 Myšlenka
Různé třídy mohou **reagovat odlišně na stejný příkaz**.  
Umožňuje napsat kód, který pracuje s nadtřídou, ale chová se podle konkrétního potomka.

### 💻 Příklad
```csharp
public class Animal
{
    public virtual void Speak()
    {
        Console.WriteLine("Zvíře vydává zvuk.");
    }
}

public class Dog : Animal
{
    public override void Speak()
    {
        Console.WriteLine("Haf!");
    }
}

public class Cat : Animal
{
    public override void Speak()
    {
        Console.WriteLine("Mňau!");
    }
}

class Program
{
    static void Main()
    {
        Animal[] animals = { new Dog(), new Cat() };

        foreach (var a in animals)
        {
            a.Speak(); // každý typ se chová jinak
        }
    }
}
```

✅ Výstup:
```
Haf!
Mňau!
```

---

## 4. 🧩 Abstrakce (Abstraction)

### 💡 Myšlenka
Abstrakce znamená **zjednodušení reality** — ukrýváme detaily a necháváme jen podstatné vlastnosti.  
Používáme abstraktní třídy nebo rozhraní, abychom definovali *co* má objekt dělat, ne *jak* to dělá.

### 💻 Příklad
```csharp
public abstract class Shape
{
    public abstract double GetArea();
}

public class Circle : Shape
{
    public double Radius { get; set; }
    public override double GetArea() => Math.PI * Radius * Radius;
}

public class Rectangle : Shape
{
    public double Width { get; set; }
    public double Height { get; set; }
    public override double GetArea() => Width * Height;
}

class Program
{
    static void Main()
    {
        Shape s1 = new Circle { Radius = 3 };
        Shape s2 = new Rectangle { Width = 4, Height = 5 };

        Console.WriteLine(s1.GetArea()); // kruh
        Console.WriteLine(s2.GetArea()); // obdélník
    }
}
```

✅ `Shape` říká, že „každý tvar má umět spočítat plochu“, ale *neřeší jak* — to dělají potomci.

---

## 🧾 Shrnutí pilířů

| Pilíř | Cíl | Klíčové slovo v C# | Výhoda |
|-------|------|--------------------|---------|
| **Zapouzdření** | Skrýt data a řídit přístup | `private`, `protected`, `public`, `get; set;` | Bezpečnost, konzistence |
| **Dědičnost** | Sdílení chování | `:` (např. `class Dog : Animal`) | Znovupoužití kódu |
| **Polymorfismus** | Různé chování pod jedním rozhraním | `virtual`, `override`, `abstract` | Flexibilita, rozšiřitelnost |
| **Abstrakce** | Omezit složitost, definovat rozhraní | `abstract`, `interface` | Jasná struktura, čitelnost |

---

📘 Tento přehled shrnuje základy objektově orientovaného programování v C#.  
Vhodný pro začátečníky i středně pokročilé programátory.

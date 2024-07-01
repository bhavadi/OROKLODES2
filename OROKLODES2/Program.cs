using System;
using System.Collections.Generic;

// Alap Jármű osztály
public abstract class Jarmu
{
    public string Azonosito { get; private set; }
    public int GyartasiEv { get; private set; }
    public string Rendszam { get; set; }
    public double Fogyasztas { get; set; } // liter / 100 km
    public double OsszKm { get; private set; }
    public bool Szabad { get; private set; }
    public double AlapDij { get; set; }

    public Jarmu(string azonosito, int gyartasiEv, string rendszam, double fogyasztas, double alapDij)
    {
        Azonosito = azonosito;
        GyartasiEv = gyartasiEv;
        Rendszam = rendszam;
        Fogyasztas = fogyasztas;
        AlapDij = alapDij;
        Szabad = true;
    }

    public void Fuvar(int km, double benzinAr)
    {
        if (!Szabad)
        {
            throw new InvalidOperationException("A jármű nem szabad.");
        }
        OsszKm += km;
        Szabad = false;
    }

    public void Visszahoz()
    {
        Szabad = true;
    }

    public double UtiKoltseg(int km, double benzinAr)
    {
        return (km * Fogyasztas / 100) * benzinAr;
    }

    public abstract double BerletiDij(int km, double benzinAr);

    public override string ToString()
    {
        return $"{Azonosito}, {GyartasiEv}, {Rendszam}, {OsszKm} km, {(Szabad ? "Szabad" : "Foglalt")}";
    }
}

// Busz osztály
public class Busz : Jarmu
{
    public int FerohelyekSzama { get; set; }
    public static double FerohelySzorzok { get; set; } = 1000; // Egységes szorzó az összes buszra

    public Busz(string azonosito, int gyartasiEv, string rendszam, double fogyasztas, double alapDij, int ferohelyekSzama)
        : base(azonosito, gyartasiEv, rendszam, fogyasztas, alapDij)
    {
        FerohelyekSzama = ferohelyekSzama;
    }

    public override double BerletiDij(int km, double benzinAr)
    {
        double utiKoltseg = UtiKoltseg(km, benzinAr);
        return AlapDij + (utiKoltseg * 1.1) + (FerohelyekSzama * FerohelySzorzok); // Haszonkulcs 10%
    }

    public override string ToString()
    {
        return base.ToString() + $", {FerohelyekSzama} férőhely";
    }
}

// Teherautó osztály
public class Teherauto : Jarmu
{
    public double Teherbiras { get; set; } // tonnában
    public static double TeherbirasSzorzok { get; set; } = 500; // Egységes szorzó az összes teherautóra

    public Teherauto(string azonosito, int gyartasiEv, string rendszam, double fogyasztas, double alapDij, double teherbiras)
        : base(azonosito, gyartasiEv, rendszam, fogyasztas, alapDij)
    {
        Teherbiras = teherbiras;
    }

    public override double BerletiDij(int km, double benzinAr)
    {
        double utiKoltseg = UtiKoltseg(km, benzinAr);
        return AlapDij + (utiKoltseg * 1.1) + (Teherbiras * TeherbirasSzorzok); // Haszonkulcs 10%
    }

    public override string ToString()
    {
        return base.ToString() + $", {Teherbiras} tonna teherbírás";
    }
}

// Főprogram
class Program
{
    static void Main()
    {
        List<Jarmu> jarmuPark = new List<Jarmu>();

        // Néhány jármű hozzáadása
        jarmuPark.Add(new Busz("B001", 2010, "ABC-123", 20, 5000, 50));
        jarmuPark.Add(new Teherauto("T001", 2012, "XYZ-789", 30, 7000, 10));

        // Járművek listázása
        Console.WriteLine("Járművek a parkban:");
        foreach (var jarmu in jarmuPark)
        {
            Console.WriteLine(jarmu);
        }

        // Fuvar hozzáadása
        Console.WriteLine("\nFuvar hozzáadása:");
        jarmuPark[0].Fuvar(150, 450);
        Console.WriteLine(jarmuPark[0]);
        Console.WriteLine($"Bérleti díj: {jarmuPark[0].BerletiDij(150, 450)}");

        // Jármű visszahozása
        jarmuPark[0].Visszahoz();
        Console.WriteLine(jarmuPark[0]);

        // Fuvar hozzáadása teherautóval
        Console.WriteLine("\nFuvar hozzáadása teherautóval:");
        jarmuPark[1].Fuvar(200, 450);
        Console.WriteLine(jarmuPark[1]);
        Console.WriteLine($"Bérleti díj: {jarmuPark[1].BerletiDij(200, 450)}");

        // Jármű visszahozása
        jarmuPark[1].Visszahoz();
        Console.WriteLine(jarmuPark[1]);

        Console.ReadKey();
    }
}

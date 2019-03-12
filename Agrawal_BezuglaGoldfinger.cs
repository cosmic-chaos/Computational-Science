using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanetaryPirates
{
    class TestAsteroidFactory : AsteroidFactory
    {
        public override Asteroid MakeAsteroidWithGold()
        {
            Asteroid asteroid;
            List<Material> material = new List<Material>();
            material.Add(Materials.Gold);
            material.Add(Materials.SiliconDioxide);
            material.Add(Materials.Carbon);
            material.Add(Materials.Water);
            material.Add(Materials.Iron);
            material.Add(Materials.Nickel);
            material.Add(Materials.Magnesium);
            material.Add(Materials.Cobalt);
            material.Add(Materials.Uranium);
            bool creating = true;
            do
            {
                int originalRadius = random.Next(0, 100);
                asteroid = new Asteroid(originalRadius, material[random.Next(8)]);
                for (int i = 0; i < random.Next(8); i++)
                {
                    int x = random.Next(-1 * originalRadius, originalRadius);
                    int y = random.Next(-1 * (int)Math.Floor(Math.Sqrt(10000 - x * x)), (int)Math.Floor(Math.Sqrt(10000 - x * x)));
                    int z = random.Next(-1 * (int)Math.Floor(Math.Sqrt(10000 - x * x)), (int)Math.Floor(Math.Sqrt(10000 - x * x)));
                    Coordinate center = new Coordinate(x, y, z);
                    int newRadius = (int)Math.Floor(100 - center.MakeVector().Magnitude());
                    if (newRadius > 0)
                    {
                        int r = random.Next(0, newRadius);
                        asteroid.AddShape(new Sphere(center, material[random.Next(8)], r));
                    }
                }
                bool golding = true;
                while(golding)
                {
                    int xG = random.Next(-1 * originalRadius, originalRadius);
                    int yG = random.Next(-1 * (int)Math.Floor(Math.Sqrt(10000 - xG * xG)), (int)Math.Floor(Math.Sqrt(10000 - xG * xG)));
                    int zG = random.Next(-1 * (int)Math.Floor(Math.Sqrt(10000 - xG * xG)), (int)Math.Floor(Math.Sqrt(10000 - xG * xG)));
                    Coordinate centerG = new Coordinate(xG, yG, zG);
                    int newRadiusG = (int)Math.Floor(100 - centerG.MakeVector().Magnitude());
                    if (newRadiusG > 0)
                    {
                        int r = random.Next(0, newRadiusG);
                        asteroid.AddShape(new Sphere(centerG, material[0], r));
                        golding = false;
                    }
                }
                var reflectancy = asteroid.GetAverageReflectance();
                var mass = asteroid.GetMass()/2000;

                var radius = asteroid.GetMaxRad();
                var centerMass = asteroid.GetCenterOfMass().X;

                var modReflectancy = 820.313 * Math.Pow(reflectancy, 4) - 1662.62 * reflectancy * reflectancy * reflectancy + 1125.17 * reflectancy * reflectancy - 81.6402 * reflectancy + 8.88492; //
                var modMass = 8.34288e-38 * Math.Pow(mass, 6) - 5.01129e-30 * Math.Pow(mass, 5) + 1.15266e-22 * Math.Pow(mass, 4) - 1.28265e-15 * mass * mass * mass + 6.94875e-9 * mass * mass - 0.017904 * mass + 16155.7; //

                var modRadius = 5000 - 124.3969 * radius - 2.977055 * radius * radius + 0.1318183 * radius * radius * radius - 0.001601738 * radius * radius * radius * radius + 0.000006278571 * radius * radius * radius * radius * radius; //
                var modCenterMass = (489 * centerMass * centerMass * centerMass * centerMass) / 5000000 + (481 * centerMass * centerMass * centerMass) / 62500 - (6973 * centerMass * centerMass) / 10000 + (2327 * centerMass) / 50 + 9843; //

                var relatedModMass = -(8410379 * Math.Pow(modMass, 4)) / 9894706676400000000000000.0 - (838801 * modMass * modMass * modMass) / 382922085000000000 - (1699379619577 * modMass * modMass) / 9894706676400000000 - (143522515993 * modMass) / 77302395909375 + 15227188723 / 149241428; //
                var relatedModCenterMass = -2.38103e-25 * Math.Pow(modCenterMass, 7) + 2.3473e-20 * Math.Pow(modCenterMass, 6) + -8.92679e-16 * Math.Pow(modCenterMass, 5) + 1.63324e-11 * Math.Pow(modCenterMass, 4) - 1.47561e-7 * Math.Pow(modCenterMass, 3) + 0.000635326 * modCenterMass * modCenterMass - 1.53641 * modCenterMass + 4477.78; //
                //Debug.Print("Mass " + mass + " Reflect " + reflectancy + " \n and modified Mass is " + modMass + " and modified Reflect is " + modReflectancy + " \n and the relation value is " + relatedModMass);
                //Debug.Print("The dif is " + Math.Abs(relatedModMass - modReflectancy));
                //Debug.Print("Center of Mass " + centerMass + " Radius " + radius);
                //Debug.Print("Mod Center " + modCenterMass + " and mod Radius " + modRadius + " and relation is " + relatedModCenterMass);
                Debug.Print("The dif is " + Math.Abs(relatedModCenterMass - modRadius));
                if (Math.Abs(relatedModMass - modReflectancy) <= 15)
                { 
                    if (Math.Abs(relatedModCenterMass - modRadius) <= 750)
                    {
                        //Debug.Print(modMass + " is the modMass " + relatedModMass + " is the related mod mass and " + modReflectancy + " is the mod reflectivity which it should equal ");
                        creating = false;
                        //var masss = asteroid.GetMass()/60;
                        //var modifiedMass = 8.34288e-38 * Math.Pow(masss, 6) - 5.01129e-30 * Math.Pow(masss, 5) + 1.15266e-22 * Math.Pow(masss, 4) - 1.28265e-15 * masss * masss * masss + 6.94875e-9 * masss * masss - 0.017904 * masss + 16155.7;
                        //Debug.Print("The mass is " + masss + " and so the modified mass is " + modifiedMass);
                        //var reflect = asteroid.GetAverageReflectance();
                        //var modR = 820.313 * Math.Pow(reflect, 4) - 1662.62 * reflect * reflect * reflect + 1125.17 * reflect * reflect - 81.6402 * reflect + 8.88492;
                        //Debug.Print("The reflectancy is " + reflect + " and so the modified reflectancy is " + modR);
                    }
                }
            } while (creating);

            return asteroid;
        }

        public override Asteroid MakeAsteroidWithoutGold()
        {
            Asteroid asteroid;
            List<Material> material = new List<Material>();
            material.Add(Materials.SiliconDioxide);
            material.Add(Materials.Carbon);
            material.Add(Materials.Water);
            material.Add(Materials.Iron);
            material.Add(Materials.Nickel);
            material.Add(Materials.Magnesium);
            material.Add(Materials.Cobalt);
            material.Add(Materials.Uranium);
            bool creating = true;
            do
            {
                int originalRadius = random.Next(0, 100);
                asteroid = new Asteroid(originalRadius, material[random.Next(7)]);
                for (int i = 0; i < random.Next(8); i++)
                {
                    int x = random.Next(-1 * originalRadius, originalRadius);
                    int y = random.Next(-1 * (int)Math.Floor(Math.Sqrt(10000 - x * x)), (int)Math.Floor(Math.Sqrt(10000 - x * x)));
                    int z = random.Next(-1 * (int)Math.Floor(Math.Sqrt(10000 - x * x)), (int)Math.Floor(Math.Sqrt(10000 - x * x)));
                    Coordinate center = new Coordinate(x, y, z);
                    int newRadius = (int)Math.Floor(100 - center.MakeVector().Magnitude());
                    if (newRadius > 0)
                    {
                        int r = random.Next(0, newRadius);
                        asteroid.AddShape(new Sphere(center, material[random.Next(7)], r));
                    }
                }
                int xG = random.Next(-1 * originalRadius, originalRadius);
                int yG = random.Next(-1 * (int)Math.Floor(Math.Sqrt(10000 - xG * xG)), (int)Math.Floor(Math.Sqrt(10000 - xG * xG)));
                int zG = random.Next(-1 * (int)Math.Floor(Math.Sqrt(10000 - xG * xG)), (int)Math.Floor(Math.Sqrt(10000 - xG * xG)));
                Coordinate centerG = new Coordinate(xG, yG, zG);
                var reflectancy = asteroid.GetAverageReflectance();
                var mass = asteroid.GetMass() / 2000;

                var radius = asteroid.GetMaxRad();
                var centerMass = asteroid.GetCenterOfMass().X;

                var modReflectancy = 820.313 * Math.Pow(reflectancy, 4) - 1662.62 * reflectancy * reflectancy * reflectancy + 1125.17 * reflectancy * reflectancy - 81.6402 * reflectancy + 8.88492; 
                var modMass = 8.34288e-38 * Math.Pow(mass, 6) - 5.01129e-30 * Math.Pow(mass, 5) + 1.15266e-22 * Math.Pow(mass, 4) - 1.28265e-15 * mass * mass * mass + 6.94875e-9 * mass * mass - 0.017904 * mass + 16155.7; 

                var modRadius = 5000 - 124.3969 * radius - 2.977055 * radius * radius + 0.1318183 * radius * radius * radius - 0.001601738 * radius * radius * radius * radius + 0.000006278571 * radius * radius * radius * radius * radius; 
                var modCenterMass = (489 * centerMass * centerMass * centerMass * centerMass) / 5000000 + (481 * centerMass * centerMass * centerMass) / 62500 - (6973 * centerMass * centerMass) / 10000 + (2327 * centerMass) / 50 + 9843; 

                var relatedModCenterMass = 4.95384e-25 *
                Math.Pow(modCenterMass, 7) - 4.32449e-20 *
                Math.Pow(modCenterMass, 6) + 1.51001e-15 *
                Math.Pow(modCenterMass, 5) - 2.68853e-11 *
                Math.Pow(modCenterMass, 4) + 2.57731e-7 *
                modCenterMass * modCenterMass * modCenterMass - 0.00130207 * modCenterMass * modCenterMass + 3.34203 * modCenterMass - 1653.05; //

                var relatedModMass = (1367 * modMass * modMass * modMass) / 1141672000000000.0 + (122487 * modMass * modMass) / 1141672000000.0 + (1487989 * modMass) / 570836000.0 + 18380585.0 / 142709;
                if (Math.Abs(relatedModMass - modReflectancy) <= 15)
                {
                    if (Math.Abs(relatedModCenterMass - modRadius) <= 500)
                    {
                        creating = false;
                        //var masss = asteroid.GetMass();
                        //var modifiedMass = 8.34288e-38 * Math.Pow(masss, 6) - 5.01129e-30 * Math.Pow(masss, 5) + 1.15266e-22 * Math.Pow(masss, 4) - 1.28265e-15 * masss * masss * masss + 6.94875e-9 * masss * masss - 0.017904 * masss + 16155.7;
                        //Debug.Print("The mass is " + masss + " and so the modified mass is " + modifiedMass);
                        //var reflect = asteroid.GetAverageReflectance();
                        //var modR = 820.313 * Math.Pow(reflect, 4) - 1662.62 * reflect * reflect * reflect + 1125.17 * reflect * reflect - 81.6402 * reflect + 8.88492;
                        //Debug.Print("The reflectancy is " + reflect + " and so the modified reflectancy is " + modR);
                    }
                }
            } while (creating);

            return asteroid;
        }
    }
}

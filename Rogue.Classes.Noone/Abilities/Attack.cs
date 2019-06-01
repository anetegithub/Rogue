﻿namespace Rogue.Classes.Noone.Abilities
{
    using Rogue.Abilities;
    using Rogue.Abilities.Enums;
    using Rogue.Abilities.Scaling;
    using Rogue.Drawing.GUI;
    using Rogue.Map;
    using Rogue.Map.Objects;
    using Rogue.View.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Attack : Ability<Noone>
    {
        public override int Position => 0;

        public override string Name => "Атака";

        public override ScaleRate Scale => ScaleRate.Build(Entites.Enums.Scale.AbilityPower, 0.1);

        public override AbilityPosition AbilityPosition => AbilityPosition.Left;

        protected override bool CanUse(Noone @class)=> @class.Actions > 0;

        protected override void Use(GameMap gameMap, Avatar avatar, Noone @class)
        {
            Console.WriteLine("use attack");

            @class.Actions -= 1;

            var rangeObject = new MapObject
            {
                Position = new Physics.PhysicalPosition
                {
                    X = avatar.Position.X - ((avatar.Size.Width * 2.5) / 2),
                    Y = avatar.Position.Y - ((avatar.Size.Height * 2.5) / 2)
                },
                Size = avatar.Size
            };

            rangeObject.Size.Height *= 2.5;
            rangeObject.Size.Width *= 2.5;

            var enemies = gameMap.Enemies(rangeObject);

            foreach (var enemy in enemies)
            {
                var value = (long)this.Value;

                enemy.Enemy.HitPoints -= value;

                if (enemy.Enemy.HitPoints <= 0)
                {
                    enemy.Die?.Invoke();
                }

                var critical = value > 10;

                this.UseEffects(new List<ISceneObject>()
                {
                    new PopupString(value.ToString()+(critical ? "!" : ""), critical ? ConsoleColor.Red : ConsoleColor.White,enemy.Location,25,critical ? 19 : 17,0.06)
                });
            }
        }

        protected override void Dispose(GameMap gameMap, Avatar avatar, Noone @class) { }

        public override double Value => Rogue.Random.Next(3,17);
    }
}
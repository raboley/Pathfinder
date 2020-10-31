using System;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;
using Pathfinder.Annotations;

namespace Pathfinder.People
{
    public class Person : IEntity, INotifyPropertyChanged, IEquatable<Person>
    {
        private string _mapName;
        private string _name;
        private Vector3 _position;

        public Person(int id, string name, Vector3 position)
        {
            Name = name;
            Position = position;
            Id = id;
        }

        public int Id { get; set; }

        public string Name
        {
            get => _name;
            set
            {
                if (value == _name) return;
                _name = value;
                OnPropertyChanged();
            }
        }

        public Vector3 Position
        {
            get => _position;
            set
            {
                if (value.Equals(_position)) return;
                _position = value;
                OnPropertyChanged();
            }
        }

        public string MapName
        {
            get => _mapName;
            set
            {
                if (value == _mapName) return;
                _mapName = value;
                OnPropertyChanged();
            }
        }

        public bool Equals(Person other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _position.Equals(other._position) && _mapName == other._mapName && _name == other._name &&
                   Id == other.Id;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is Person other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = _position.GetHashCode();
                hashCode = (hashCode * 397) ^ (_mapName != null ? _mapName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (_name != null ? _name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Id;
                return hashCode;
            }
        }

        public static bool operator ==(Person left, Person right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Person left, Person right)
        {
            return !Equals(left, right);
        }
    }
}
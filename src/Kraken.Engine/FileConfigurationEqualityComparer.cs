using System.Collections.Generic;

namespace Kraken.Engine
{
    public sealed class FileConfigurationEqualityComparer : IEqualityComparer<FileConfiguration>
    {
        public bool Equals(FileConfiguration x, FileConfiguration y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.Id.Equals(y.Id) && x.ComponentName == y.ComponentName && x.PathToFile == y.PathToFile && x.OctopusProject == y.OctopusProject && x.OctopusArtifactName == y.OctopusArtifactName && Equals(x.Substitutions, y.Substitutions) && x.IsSubstitutionsOnly == y.IsSubstitutionsOnly;
        }

        public int GetHashCode(FileConfiguration obj)
        {
            unchecked
            {
                var hashCode = obj.Id.GetHashCode();
                hashCode = (hashCode * 397) ^ (obj.ComponentName != null ? obj.ComponentName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.PathToFile != null ? obj.PathToFile.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.OctopusProject != null ? obj.OctopusProject.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.OctopusArtifactName != null ? obj.OctopusArtifactName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.Substitutions != null ? obj.Substitutions.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ obj.IsSubstitutionsOnly.GetHashCode();
                return hashCode;
            }
        }
    }
}
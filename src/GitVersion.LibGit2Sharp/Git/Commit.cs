using System;
using System.Collections.Generic;
using System.Linq;
using GitVersion.Helpers;

namespace GitVersion
{
    internal sealed class Commit : ICommit
    {
        private static readonly LambdaEqualityHelper<ICommit> equalityHelper = new(x => x.Id);
        private static readonly LambdaKeyComparer<ICommit, string> comparerHelper = new(x => x.Sha);

        private readonly LibGit2Sharp.Commit innerCommit;

        internal Commit(LibGit2Sharp.Commit innerCommit) => this.innerCommit = innerCommit;

        public int CompareTo(ICommit other) => comparerHelper.Compare(this, other);
        public bool Equals(ICommit other) => equalityHelper.Equals(this, other);
        public override bool Equals(object obj) => Equals((obj as ICommit)!);
        public override int GetHashCode() => equalityHelper.GetHashCode(this);
        public override string ToString()
        {
            return $"{Id.ToString(7)} {innerCommit.MessageShort}";
        }
        public static implicit operator LibGit2Sharp.Commit(Commit d) => d.innerCommit;

        public IEnumerable<ICommit> Parents => innerCommit.Parents.Select(parent => new Commit(parent));

        public string Sha => innerCommit.Sha;

        public IObjectId Id => new ObjectId(innerCommit.Id);

        public DateTimeOffset When => innerCommit.Committer.When;

        public string Message => innerCommit.Message;
    }
}

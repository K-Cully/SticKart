// -----------------------------------------------------------------------
// <copyright file="ScoreNamePair.cs" company="None">
// Copyright Keith Cully 2013.
// </copyright>
// -----------------------------------------------------------------------

namespace SticKart.Game.Level
{
    using System;
    using System.Collections;

    /// <summary>
    /// A wrapper to tie a score and name together.
    /// </summary>
    public class ScoreNamePair : IComparable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScoreNamePair"/> class.
        /// </summary>
        public ScoreNamePair()
        {
            this.Score = 0;
            this.Name = "ZZZ";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScoreNamePair"/> class.
        /// </summary>
        /// <param name="score">The score value.</param>
        /// <param name="name">The name value.</param>
        public ScoreNamePair(int score, string name)
        {
            this.Score = score;
            this.Name = name;
        }

        /// <summary>
        /// Gets or sets the score.
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Retrieves the <see cref="IComparer"/> instance for an ascending sort.
        /// </summary>
        /// <returns>The ascending comparer.</returns>
        public static IComparer SortScoreAscending()
        {
            return (IComparer)new SortScoreAscendingHelper();
        }

        /// <summary>
        /// Retrieves the <see cref="IComparer"/> instance for a descending sort.
        /// </summary>
        /// <returns>The descending comparer.</returns>
        public static IComparer SortScoreDescending()
        {
            return (IComparer)new SortScoreDescendingHelper();
        }

        /// <summary>
        /// Overrides the default ToString behaviour.
        /// </summary>
        /// <returns>The score name pair as its string representation.</returns>
        public override string ToString()
        {
            int finalLength = 24;
            int stringLength = this.Name.Length + this.Score.ToString().Length;
            int paddingLength = finalLength - stringLength > 0 ? finalLength - stringLength : 0;
            return string.Format("{0}" + new string(' ', paddingLength) + "{1}", this.Name, this.Score);
        }

        /// <summary>
        /// Compares this instance to a specified <see cref="ScoreNamePair"/> instance.
        /// </summary>
        /// <param name="other">The other instance to compare to.</param>
        /// <returns>An indication of the two object's relative values.</returns>
        public int CompareTo(object other)
        {
            ScoreNamePair otherPair = (ScoreNamePair)other;
            if (this.Score == otherPair.Score)
            {
                return this.Name.CompareTo(otherPair.Name);
            }
            else
            {
                return otherPair.Score.CompareTo(this.Score);
            }
        }

        #region helpers

        /// <summary>
        /// Nested class to perform descending sort on score property.
        /// </summary>
        private class SortScoreDescendingHelper : IComparer
        {
            /// <summary>
            /// Compares two <see cref="ScoreNamePair"/> instances for a descending sort.
            /// </summary>
            /// <param name="objectOne">The first object.</param>
            /// <param name="objectTwo">The second object.</param>
            /// <returns>The result of the comparison.</returns>
            int IComparer.Compare(object objectOne, object objectTwo)
            {
                if ((objectOne as ScoreNamePair).Score < (objectTwo as ScoreNamePair).Score)
                {
                    return 1;
                }
                else if ((objectOne as ScoreNamePair).Score > (objectTwo as ScoreNamePair).Score)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Nested class to perform ascending sort on score property.
        /// </summary>
        private class SortScoreAscendingHelper : IComparer
        {
            /// <summary>
            /// Compares two <see cref="ScoreNamePair"/> instances for an ascending sort.
            /// </summary>
            /// <param name="objectOne">The first object.</param>
            /// <param name="objectTwo">The second object.</param>
            /// <returns>The result of the comparison.</returns>
            int IComparer.Compare(object objectOne, object objectTwo)
            {
                if ((objectOne as ScoreNamePair).Score > (objectTwo as ScoreNamePair).Score)
                {
                    return 1;
                }
                else if ((objectOne as ScoreNamePair).Score < (objectTwo as ScoreNamePair).Score)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            }
        }

        #endregion
    }
}

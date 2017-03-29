using GalaSoft.MvvmLight;
using System;
using System.Linq;

namespace Books_in_MVVM_LINQ.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        public string outputTextBox { get; set; }
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            // Entity Framework DBContext
            BooksEntities dbcontext = new BooksEntities();
            //get authors and and titles, sorted by title
            var TitlesAndAuthors =
                from author in dbcontext.Authors
                from book in author.Titles
                orderby book.Title1
                select new { book.Title1, author.FirstName, author.LastName };

            outputTextBox += ("Titles and Authors:");

            //display authors and and titles, sorted by 
            foreach (var element in TitlesAndAuthors)
            {
                outputTextBox += (
                   String.Format("\r\n\t{0,-50} {1} {2}",
                      element.Title1, element.FirstName, element.LastName));
            } // end foreach

            outputTextBox += ("\r\n\r\n");

            //get authors and titles, sorted by authors
            var byAuthors =
                from author in dbcontext.Authors
                from book in author.Titles
                orderby book.Title1, author.FirstName, author.LastName
                select new { book.Title1, author.FirstName, author.LastName };

            outputTextBox += ("Authors and titles with authors sorted for each title:");

            //display orderd by authors for each book.
            foreach (var element in byAuthors)
            {
                outputTextBox += (
                   String.Format("\r\n\t{0,-50} {1} {2}",
                      element.Title1, element.FirstName, element.LastName));
            } // end foreach

            // get authors and titles of each book 
            // they co-authored; group by author
            var authorsByTitle =
               from book in dbcontext.Titles
               orderby book.Title1
               select new
               {
                   Title = book.Title1,
                   Authors =
                     from author in book.Authors
                     orderby author.LastName, author.FirstName
                     select author.FirstName + " " + author.LastName
               };

            outputTextBox += ("\r\n\r\nTitles grouped by author:");

            // display titles written by each author, grouped by author
            foreach (var Titles in authorsByTitle)
            {
                // display author's name
                outputTextBox += ("\r\n\t" + Titles.Title + ":");
                foreach (var Author in Titles.Authors)
                {
                    outputTextBox += ("\r\n\t" + Author);
                }
            } // end outer foreach
            this.RaisePropertyChanged(() => outputTextBox);

        }
    }
}
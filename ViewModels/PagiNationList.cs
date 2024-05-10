using System.Drawing.Printing;

namespace Wolmart.MVC.ViewModels
{
    public class PagiNationList<T> : List<T>
    {
        public PagiNationList(IQueryable<T> entities, int page, int totalPages)
        {
            TotalPages = totalPages;
            TotalItems = entities.Count();
            PageIndex = page;

            int start = PageIndex - 2;
            int end = PageIndex + 2;

            if (start <= 0)
            {
                end = end - (start - 1);
                start = 1;
            }

            if (end > totalPages)
            {
                end = totalPages;

                if (end > 5)
                {
                    start = end - 4;
                }
            }

            Start = start;
            End = end;

            AddRange(entities);
        }

        public int TotalPages { get; set; }
        public int TotalItems { get; set; }

        public int PageIndex { get; set; }

        public bool HasNext => PageIndex < TotalPages;
        public bool HasPrev => PageIndex > 1;

        public int Start { get; private set; }
        public int End { get; private set; }

        public static PagiNationList<T> Create(IQueryable<T> entities, int page, int pageItemCount)
        {
            int totalPages = (int)Math.Ceiling((decimal)entities.Count() / pageItemCount);

            if (page < 1 || page > totalPages)
            {
                page = 1;
            }

            entities = entities.Skip((page - 1) * pageItemCount).Take(pageItemCount);

            return new PagiNationList<T>(entities, page, totalPages);
        }

    }
}

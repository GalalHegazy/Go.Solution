namespace Go.APIs.Helpers
{
    public class Pagination<T>
    {
        public int pageindex {  get; set; } 
        public int pagesize { get; set; }
        public int count { get; set; }
        public IReadOnlyList<T> data { get; set; }

        public Pagination(int Pageindex, int Pagesize, int Count, IReadOnlyList<T> Data)
        {
            pageindex = Pageindex;
            pagesize = Pagesize;
            count = Count;
            data = Data;
        }
    }
}

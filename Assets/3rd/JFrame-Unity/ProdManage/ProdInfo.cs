namespace JFrame.Common
{
    public struct ProdInfo
    {

        public string uid;
        public ProdStatus status;
        public int number;
        public ProdInfo(string uid, ProdStatus status, int number)
        {
            this.uid = uid;
            this.status = status;
            this.number = number;
        }

    }

}
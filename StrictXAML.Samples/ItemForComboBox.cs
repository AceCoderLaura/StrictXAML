namespace StrictXAML.Samples
{
    public class ItemForComboBox
    {
        public ItemForComboBox(string name, int itemNumber)
        {
            Name = name;
            ItemNumber = itemNumber;
        }

        public string Name { get; set; }
        public int ItemNumber { get; set; }
    }
}
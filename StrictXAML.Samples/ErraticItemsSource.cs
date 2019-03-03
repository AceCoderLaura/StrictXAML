using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using System.Timers;
using System.Windows.Markup;
using Timer = System.Timers.Timer;

namespace StrictXAML.Samples
{
    /// <inheritdoc />
    /// <summary>
    /// An ItemsSource that is all over the place.
    /// </summary>
    public class ErraticItemsSource : MarkupExtension
    {
        public ErraticItemsSource()
        {
            _captureContext = SynchronizationContext.Current;
            var t = new Timer(400);
            t.Elapsed += JumbleData;
            t.Start();
        }

        private readonly SynchronizationContext _captureContext;

        private void JumbleData(object state, ElapsedEventArgs elapsedEventArgs)
        {
            _captureContext.Post(x =>
            {
                var random = (Random) x;
                var options = (NotifyCollectionChangedAction[]) typeof(NotifyCollectionChangedAction).GetEnumValues();
                var actionNumber = random.Next(options.Length);
                switch (options[actionNumber])
                {
                    case NotifyCollectionChangedAction.Move:
                    case NotifyCollectionChangedAction.Replace:
                    case NotifyCollectionChangedAction.Add:
                        var thingNamesLength = ItemForComboBoxDirectory.Length;
                        var thingNameNumber = random.Next(thingNamesLength);
                        _itemsSource.Add(ItemForComboBoxDirectory[thingNameNumber]);
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        if (!_itemsSource.Any()) break;
                        var itemNumber = random.Next(_itemsSource.Count);
                        _itemsSource.RemoveAt(itemNumber);
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        _itemsSource.Clear();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }, _random);
        }

        private static readonly ItemForComboBox[] ItemForComboBoxDirectory =
        {
            new ItemForComboBox("Foo", 2),
            new ItemForComboBox("Bar", 3),
            new ItemForComboBox("Roo", 4),
            new ItemForComboBox("Dar", 5),
            new ItemForComboBox("Yoo", 6),
            new ItemForComboBox("Mar", 7)
        };

        private readonly Random _random = new Random();

        public override object ProvideValue(IServiceProvider serviceProvider) => _itemsSource;

        private readonly ObservableCollection<ItemForComboBox> _itemsSource = new ObservableCollection<ItemForComboBox>(ItemForComboBoxDirectory);
    }
}
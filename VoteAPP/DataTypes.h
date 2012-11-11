namespace VoteAPP
{
    // To be bindable, a class must be defined within a namespace
    // and a bindable attribute needs to be applied.
    // A CompareItem represents a single item to compare.
    [Windows::UI::Xaml::Data::Bindable]
    public ref class CompareItem sealed
    {
    public:
		// Constructor.
        CompareItem(void){}

        property Platform::String^ Title;
        property Platform::String^ Price;
        property Platform::String^ Content;
        property Windows::Foundation::Uri^ Link;

    private:
		// Destructor.
        ~CompareItem(void){}
    };

    // An ItemFeed object represents a feed that contains 
    // one or more CompareItems. 
    [Windows::UI::Xaml::Data::Bindable]
    public ref class ItemFeed sealed
    {
    public:
        ItemFeed(void)
        {
            m_items = ref new Platform::Collections::Vector<CompareItem^>();
        }

        // The public members must be Windows Runtime types so that
        // the XAML controls can bind to them from a separate .winmd.
        property Platform::String^ Title;            
        property Windows::Foundation::Collections::IVector<CompareItem^>^ Items
        {
            Windows::Foundation::Collections::IVector<CompareItem^>^ get() {return m_items; }
        }

        property Platform::String^ Description;
        property Windows::Foundation::DateTime PubDate;

    private:
        ~ItemFeed(void){}

        Platform::Collections::Vector<CompareItem^>^ m_items;



    };   

}

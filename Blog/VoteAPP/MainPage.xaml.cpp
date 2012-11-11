//
// MainPage.xaml.cpp
// Implementation of the MainPage class.
//

#include "pch.h"
#include "MainPage.xaml.h"

using namespace VoteAPP;

using namespace Platform;
using namespace Windows::Foundation;
using namespace Windows::Foundation::Collections;
using namespace Windows::UI::Xaml;
using namespace Windows::UI::Xaml::Controls;
using namespace Windows::UI::Xaml::Controls::Primitives;
using namespace Windows::UI::Xaml::Data;
using namespace Windows::UI::Xaml::Input;
using namespace Windows::UI::Xaml::Media;
using namespace Windows::UI::Xaml::Navigation;

// Added as per turorial.
using namespace Windows::Web::Syndication;
using namespace concurrency;					// Deals with async. tasks.

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

MainPage::MainPage()
{
	InitializeComponent();
	itemFeed = ref new ItemFeed();		// As per tutorial.
}

/// <summary>
/// Invoked when this page is about to be displayed in a Frame.
/// </summary>
/// <param name="e">Event data that describes how this page was reached.  The Parameter
/// property is typically used to configure the page.</param>
void MainPage::OnNavigatedTo(NavigationEventArgs^ e)
{
	(void) e;	// Unused parameter
}

void MainPage::GetCompareItemData(Platform::String^ feedUriString)
{
	
/*	// Create the SyndicationClient and the target uri
    SyndicationClient^ client = ref new SyndicationClient();
    Uri^ feedUri = ref new Uri(feedUriString);

    // Create the async operation. feedOp is an 
    // IAsyncOperationWithProgress<SyndicationFeed^, RetrievalProgress>^
    auto feedOp = client->RetrieveFeedAsync(feedUri);

    // Create the task object and pass it the async operation.
    // SyndicationFeed^ is the type of the return value
    // that the feedOp operation will eventually produce.    

    // Create a "continuation" that will run when the first task completes.
    // The continuation takes the return value of the first operation,
    // and then defines its own asynchronous operation by using a lambda.
    create_task(feedOp).then([this] (SyndicationFeed^ feed) -> SyndicationFeed^
    {
        // Get the title of the feed (not the individual posts).
        feedData->Title = feed ->Title->Text;

        // Retrieve the individual posts from the feed.
        auto feedItems = feed->Items;

        // Iterate over the posts. You could also use
        // std::for_each( begin(feedItems), end(feedItems),
        // [this, feed] (SyndicationItem^ item)
        for(int i = 0; i < (int)feedItems->Size; i++)
        {         
            auto item = feedItems->GetAt(i);
            FeedItem^ feedItem = ref new FeedItem();
            feedItem->Title = item->Title->Text; 
            feedItem->PubDate = item->PublishedDate;

            feedItem->Author = item->Authors->GetAt(0)->Name; 

            if (feed->SourceFormat == SyndicationFormat::Atom10)
            {
                feedItem->Content = item->Content->Text;
            }
            else if (feed->SourceFormat == SyndicationFormat::Rss20)
            {
                feedItem->Content = item->Summary->Text;
            }
            feedData->Items->Append(feedItem);
        }

        this->DataContext = feedData;
        return feed;         
    })
    // The last continuation serves as an error handler. The
    // call to get() will surface any exceptions that were raised
    // at any point in the task chain.
    .then( [this] (concurrency::task<SyndicationFeed^> t)
    {
        try
        {
            t.get();
        }
        // SyndicationClient throws E_INVALIDARG 
        // if a URL contains illegal characters.
        catch(Platform::InvalidArgumentException^ e)
        {
            // TODO handle error. For example purposes
            // we just output error to console.
            OutputDebugString(e->Message->Data());
        }
    });
*/
}


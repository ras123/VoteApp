﻿

#pragma once
//------------------------------------------------------------------------------
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
//------------------------------------------------------------------------------

namespace Windows {
    namespace UI {
        namespace Xaml {
            namespace Controls {
                ref class Grid;
                ref class TextBlock;
                ref class ListView;
                ref class WebView;
            }
        }
    }
}

namespace VoteAPP
{
    partial ref class MainPage : public ::Windows::UI::Xaml::Controls::Page, 
        public ::Windows::UI::Xaml::Markup::IComponentConnector
    {
    public:
        void InitializeComponent();
        virtual void Connect(int connectionId, ::Platform::Object^ target);
    
    private:
        bool _contentLoaded;
    
        private: ::Windows::UI::Xaml::Controls::Grid^ Grid1;
        private: ::Windows::UI::Xaml::Controls::TextBlock^ TitleText;
        private: ::Windows::UI::Xaml::Controls::Grid^ Grid2;
        private: ::Windows::UI::Xaml::Controls::ListView^ ItemListView;
        private: ::Windows::UI::Xaml::Controls::Grid^ Grid3;
        private: ::Windows::UI::Xaml::Controls::WebView^ ContentView;
    };
}

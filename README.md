The [Microsoft Ignite Australia](http://msignite.com.au/) conference, held yearly on the Gold Coast, is Australia's premier Microsoft technology conference and attracts more than 2500 technical attendees. With Bots and Conversational interfaces being a big thing at the conference this year, [NV Interactive](https://www.nvinteractive.com/) and Microsoft decided to work together to build a [conference bot](https://channel9.msdn.com/Events/Ignite/Australia-2017/Keynote1#time=1h01m00s) for the attendees. 

During the conference the attendees have access to a web site that allows them to build a personalised schedule of sessions they plan to attend. Users are able to log in to the bot using their conference account to perform a range of actions including searching for sessions, finding answers to common questions and finding out what their next session is plus much more. 

The Ignite Conference is large with over 2500 attendees with a vast array of sessions, tracks and rooms to navigate. A delegate may opt to attend six or more sessions per day of the event. 

In the past when an attendee want's to find out what their next session is they have had to stop, get out their phone or laptop, navigate to the conference site, load the schedule page, find the current time and see what they have on next. And that's only if they have actually scheduled something - it's even harder for them if they want to find something in a track in the next slot. 

Some common conversations a user may have with the bot include:

- Where is my next session
- Add ARC201
- When is NET324
- Find me talks by Jordan Knight
- Where are the taxis
- When is lunch
- Where is the closing party
- How were you made

#### Key Technologies

- [ASP.NET](https://www.asp.net/) / [C#7](https://blogs.msdn.microsoft.com/dotnet/2016/08/24/whats-new-in-csharp-7-0/) 
- [Bot Framework BotBuilder SDK](https://docs.botframework.com/en-us/csharp/builder/sdkreference/)
- [Azure App Service](https://docs.microsoft.com/en-us/azure/app-service/)
- [Azure Redis Cache](https://docs.microsoft.com/en-au/azure/redis-cache/)
- [Language Understanding Intelligent Service](https://www.luis.ai/)
- [QnA Maker Intelligent Service](https://qnamaker.ai/)
- [Azure Search](https://docs.microsoft.com/en-au/azure/search/)
- [Bing Translation](https://docs.microsoft.com/en-us/azure/cognitive-services/Translator/translator-info-overview)
- [Arlo SDK](https://github.com/MSFTAuDX/ArloSdk)

The main Ignite conference web site back end is powered by a system called [Arlo](https://www.arlo.co/) ([Developer documentation](https://developer.arlo.co/)) ([.NET SDK](https://github.com/MSFTAuDX/ArloSdk)) which stores all the conference data including the session catalog, the speakers and the room allocations. 

Microsoft and NV worked closely to produce to bot with tight integration required between the back end systems, the bot framework and authentication. 

#### The team

On the Microsoft side were Jordan Knight (Senior Technical Evangelist [@jakkaj](https://twitter.com/jakkaj)), Tim Hill (Senior Consultant / Modern Apps) and Dipanjan Ghosh (Premier Field Engineer).

On the NV side we were  joined by Nadia MacLaren (Technical Lead), Gus Pickering (Technical Director) and Christian Delgado (Digital Project Manager). 

## Customer profile ##

NV Interactive [https://www.nvinteractive.com](https://www.nvinteractive.com) (pronounced "envy") is a digital agency and bespoke software development company mixed in to one. They produce graphic design, architecture design and user experience design as well as building mobile apps, desktop apps and various online systems. NV is based in New Zealand with offices in Auckland, Wellington and Christchurch. NV is an award winning Gold Microsoft Partner. 

NV Interactive managing Director Matt Pickering said "A bot was the next logical extension for the Ignite platform, and a perfect use case for conversational user interfaces. We knew everything about the session catalogue, the delegates’ schedule and it was a simple matter of exposing this to the Bot and authorising access to it via OAUTH.  Everything worked brilliantly and we’re looking at ways to leverage this in the platform for future Ignite events."

Ignite Australia is a technical conference targeting both IT Pros and Developers. This year the conference saw over 2500 people hosted at the Gold Coast Convention Center. There were 181 sessions delivering 14,000 minutes of content delivered by 173 speakers. Videos from the event are available on [Microsoft Channel 9](https://channel9.msdn.com/Events/Ignite/Australia-2017/) including a presentation about the event bot itself [here](https://channel9.msdn.com/Events/Ignite/Australia-2017/CLD232). 

## Problem statement ##

The decison to build a bot for Ignite Australia was based on two main drivers: a) to deliver a bot in to the hands of users to improve their conference experience and b) to drive awareness of bots via direct first hand experience of bots and how they work. 

#### Shallow Interactions ####

Language is perhaps the original UI. Users have been using the language "UI" since they first learned to talk, read and write. It is easy to ask a question using a language they know well whether that be spoke word or written text. This is opposed to the numerous different website designs and other visual interfaces that must be learned before they become intuitive.

The Ignite Bot makes it super easy to discover contextual meaningful and real-time event data.The user can just whip out their phone and open skype - no website or shortcuts to navigate and load - before typing their question using language that is natural for them. It's human centered. It's a shallow interaction that can be performed with minimal interruption. 

"What's my next session" will show the user the next session from their schedule. "What's on next" will ask the user "Which track" and provide a list of tracks to choose from - providing a simple way to find out what's coming up in a particuar track. 

### Bot Awareness ###

Whilst intelligent bots are a newer technology, they are based on existing and familiar concepts and it can be difficult for users to understand their value given their previous experiences. Most users have chatted in Skype, and some may have even used a bot before (some may have even used them years ago in IRC etc!)

By putting a bot in to attendees hands and allowing them to use it to navigate their event plan, we aimed to have something for them to use to get some real world experience of how bots operate before they went along to the various bot deep dive sessions throughout the event. 

## Solution and steps ##

The main components of the bot are: 

- [Bot WebApi code](https://github.com/MSFTAuDX/SimpleBot/blob/master/SimpleIgniteBot/SimpleIgniteBot/Controllers/MessagesController.cs)
    - [BotBuilder Nuget package](https://www.nuget.org/packages/Microsoft.Bot.Builder/)
- Synchronisation with Ignite session catalog (Arlo)
- Intelligent Services
    - [LUIS](https://www.luis.ai/)
    - [QnA Maker](https://qnamaker.ai/)
    - [Bing Translator](https://docs.microsoft.com/en-us/azure/cognitive-services/Translator/translator-info-overview)
- Search using [Azure Search](https://docs.microsoft.com/en-au/azure/search/)
- Authentication using OAuth
- Telemetry and logging using [Application Insights](https://docs.microsoft.com/en-us/azure/application-insights/)

At the beginning of the project we broke down what an event bot would look like. Imagine you could ask someone any question about the event and they have the answer immediately - what would you ask. Our team has attended and indeed run many technical events of this scale - so we sat and really thought about the features this bot needs. We also researched the capabilities of bots that have been produced for other Microsoft events such as Ignite in North America.  

We decided that users need to be able to:

- Sign in and out
- Find talks by presenters
- Review their schedule in context to now
- Add and remove items by the schedule
- Find talks in tracks (filter by track)
- Get information about a particular session (using a session code)
- Find a random session
- Get help
- Easily provide feedback
- Find out how the bot was built
- Find general event information

On top of this, the bot should have a personality and be funny - perhaps tell jokes. 

Importantly - the bot needs to be fast! We wanted the bot to respond immediately, which lead to some performance architectural decisions. 

<img alt="Overview Architecture Diagrams" src="{{ site.baseurl }}/images/2017-04-07-nv/NVIgniteBot_architecture.png" width="720">
</img>

#### High level flow ####

The basic user flow of the app is:
```
User Client (e.g. Skype) -> Bot Connector -> WebApi -> "Services"
```
There is a secondary flow that keeps the system updated with data from the Ignite Session Catalog. 

```
Timer trigger -> Azure App Service WebJob -> Arlo RESful API -> Redis Cache
```

We opted to pre-cache the entire session catalog in Redis to a) take load from the Arlo system, and b) ensure queries were answered as fast as possible. 

#### Services and fall through "levels" ####

The ignite bot is set up with a number of services - the service at each level performs a query, and if that level doesn't return a result, it will fall to the service at the next level. 

The services at each level are:

- Basic checks on text
- LUIS natural language intent
- QnA Maker query
- Azure Search full text search
- Check langauge and offer to translate

The reason for this order is that first up, we want to be able to run special commands before we do any processing (i.e. a direct match) so we do some basic checks and direct responses. The next step is figuring out what the user is asking - we use LUIS to help us figure that out. If LUIS fails to find anything, then we send the question off to the QnA maker which may find a direct answer to the users question. Failing that, we run the query through Azure Search, which is very broad. Pretty much if the user used any of the main keywords from the event nomenclature they will get a response. If that fails - we check language. It's a lot of checks to have fallen through (LUIS, Search etc.) - so perhaps they are not speaking English at all, and we want to accommodate that (more on this below.)

It should be noted that we don't check language early on in the flow because language checking is a slow operation and it would slow down the general experience for all users regardless of the language they are using. Using this flow, we can make a reasonable assumption that if LUIS, QnA Maker or Search get a hit, then the user is speaking English (or the translation system has been enabled!). 

Also note that once language preferences have been set, the actual translation process is much faster than the language detection process and the user will have a speedy experience even if the system is translating in and out from English to their preferred language. 

## Getting Started ##

The bot is written mostly using the .NET Framework 4.6 / C# (7) and WebApi. We could have just as easily built the bot using Node.js and most of the concepts here would work if translated to JavaScript.

We opted for a full dependency injection (DI) / Inversion of Control (IOC) model using Autofac to help keep our code neat. 

To assist with documentation and education we created two examples - The [BotTranslator](https://github.com/MSFTAuDX/BotTranslator) which is built using DI, and the [SimpleBot](https://github.com/MSFTAuDX/SimpleBot) sample which has the DI stripped out for simplicity, although it maintains some of the modularity left over from the separation of the models for DI. Each of these examples are a good basis for replicating the Ignite bot. 

To get started you can follow these steps. 

### 1. Install Visual Studio

[Grab a copy of Visual Studio](https://www.visualstudio.com/downloads/) and install - make sure to include the "ASP.NET and Web development" workload in the install feature option dialog. If you've already installed Visual Studio and cannot see the the "Web" tab in the create new project dialog box - scroll to the bottom of the left pane and select "Open Visual Studio Installer" and proceed to add the workload. 

<img alt="Visual Studio Workloads" src="{{ site.baseurl }}/images/2017-04-07-nv/VisualStudioWorkloads.png" width="320">
</img>

### 2. Create your new project

Create a new ASP.NET WebAPI project. Select "Create New Project" from the Visual Studio 2017 launch screen, select Web and choose "ASP.NET Web Application (.NET Framework), choose a folder for your new project and press OK. On the next screen select Web API and press OK. 

<img alt="Visual Studio Workloads" src="{{ site.baseurl }}/images/2017-04-07-nv/WebAPICreate.png" width="320">
</img>

### 3. Install the BotBuilder Nuget Package

The base classes and boiler plate code we used to build this bot are part of the [BotBuilder](https://www.nuget.org/packages/Microsoft.Bot.Builder/) Nuget package. It includes the capability to work with bot sessions, respond to user events and to easily call LUIS services amongst other things. 

You can install this from the Package Manager console by typing: 

```Install-Package Microsoft.Bot.Builder```

You can also use the Package Manager GUI and search for BotBuilder. 

Alternatively you can base your new bot on one of the bot samples on GitHub [here](https://github.com/Microsoft/BotBuilder-Samples) [this one is good for LUIS based bots](https://github.com/Microsoft/BotBuilder-Samples/tree/master/CSharp/intelligence-LUIS) or you can grab the [SimpleBot](https://github.com/MSFTAuDX/SimpleBot) sample and start with that. 

### Special note about Node.js 
Although in this document we're dealing with C# based bots, for node you can install the BotBuilder npm package to get started:

```npm install BotBuilder --save```

Then you may utilise the bot builder in this way: 

```var botbuilder = require("botbuilder");```

The full node.js BotBuilder documentation can be found [here](https://docs.botframework.com/en-us/node/builder/overview/).

## WebApi ##

The core of the bot - the code - all lives in a single WebApi service. The bot is accessed by the Bot Framework via a restful http call to an endpoint. 

The base pattern is one of the many examples available on the [BotBuilder-Samples](https://github.com/Microsoft/BotBuilder-Samples) Github repo.

The [MessagesController](https://github.com/MSFTAuDX/SimpleBot/blob/master/SimpleIgniteBot/SimpleIgniteBot/Controllers/MessagesController.cs) is where the bot is basically bootstrapped. 

```csharp
public async Task<HttpResponseMessage> Post([FromBody] Activity activity)
{
    
}
```

Basic text checks happen in the MessagesController. First we check to see if the ```activity``` is a ```Message``` and proceed to check the text to see if we need to perform a command. 

Some commands we can perform are ```ping``` (check the bot is there!) and ```command language``` which will reset the language (more on that later). 

```csharp
if (activity == null)
{
    //perform some logging
}
else if (activity.Type == ActivityTypes.Message)
{
    var text = activity.Text;

    if (text.ToLowerInvariant() == "ping")
    {
        ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
        Activity reply = activity.CreateReply("pong");
        await connector.Conversations.ReplyToActivityAsync(reply);
    }
```

You can see from this example how we're directly responding to user input in the most basic of ways by calling ```connector.Conversations.ReplyToActivityAsync(reply)```. Very non-intelligent so far, but simple and allows for the user to send basic commands and queries to the bot.  

If nothing is matched, then we pass through to the second service level - LUIS. 

### Level 2: LUIS - Language Understanding Intelligent Service ###

Users can ask questions in a variety of ways - it's part of the beauty of a chat based application... but traditionally it's been difficult for developers to understand a users intent for much more than a very few terse terms and that's where LUIS steps in. 

LUIS takes the user's query, and turns it in to easily machine readable JSON data. 

<img alt="LUIS Json Output Example" src="{{ site.baseurl }}/images/2017-04-07-nv/jsonexample.png" width="320">
</img>

LUIS is also able to parse out the entities from you query - for example if you ask ```"Find me talks by Jordan Knight"``` the system will be able to return the intent ```FindTalkPerson``` and the entities ```FirstName```=Jordan and ```LastName```=Knight. 

The queries are performed over standard http - just call the end point and you'll get a result. 

You set up your intents and entities at [luis.ai](http://www.luis.ai). Full documentation is [available here](https://www.luis.ai/home/help). The Ignite Australia 2017 LUIS project json export can be found [here](https://gist.github.com/jakkaj/02f8f7d2152c0e9403342be608e05754). 


When creating LUIS intents you can bolster their accuracy by using phrase lists. These lists are words that will match to certain entities, such as names, or track titles. The sample json we've provided had had the names lists removed.

Here is the json example of a phrase lists for the conference tracks to help give confidence to the model when matching queries that involve searching tracks. 

```json
 {
      "name": "Tracks",
      "mode": true,
      "words": "Datacenter and Infrastructure Management,Cloud,. NET,Productivity,Data and Analytics,Keynote,Kick Off,Microsoft Ignite Learning Zone,Open Source,Windows,Innovation,Social,Architecture,Expo,Hack",
      "activated": true
    }
``` 

To add these lists in the LUIS UI - select the "Features" tab. It's recommended you auto generate these phrase lists as part of your build and programmatically upload them via the [LUIS API](https://dev.projectoxford.ai/docs/services/56d95961e597ed0f04b76e58/operations/5739a8c71984550500affdfa). 

<img alt="LUIS Json Output Example" src="{{ site.baseurl }}/images/2017-04-07-nv/LuisPortal.PNG" width="720">
</img>

The BotBuilder contains boilerplate classes that assist with sending queries to LUIS and also in finding the correct code to run when an intent is detected. 

```csharp
[Serializable]
[LuisModel("<luis.ai app id>", "app secret")]
public partial class LuisModel : LuisDialog<object>, IDialog<object>
```

The basic premise is that you create a ```Serializable``` class based on ```LuisDialog<object>``` then decorate intent call back methods with ```  [LuisIntent("IntentName")]``` attributes. 

```csharp

[LuisIntent("FindTalkPerson")]
public async Task PersonFinder(IDialogContext context, LuisResult result)
{
    EntityRecommendation recommendationFirst = null;
    EntityRecommendation recommendationLast = null;

    result.TryFindEntity("FirstName", out recommendationFirst);
    result.TryFindEntity("LastName", out recommendationLast);

    if (recommendationFirst != null || recommendationLast != null)
    {
        var entityFirstName = recommendationFirst?.Entity ?? string.Empty;
        var entityLastName = recommendationLast?.Entity ?? string.Empty;
```
(sample code on [Github](https://github.com/MSFTAuDX/SimpleBot/blob/master/SimpleIgniteBot/SimpleIgniteBot/Controllers/MessagesController.cs))

The next stage is to parse out the entities. Here you can see we're looking for ```FirstName``` and ```LastName``` entities. Once we have those entities we can go off and search the back end database for that presenter and return cards for the sessions. 

#### Activity Cards ####

The Ignite Bot makes heavy use of Activity Cards to show session information. Further sample code for how the session cards work is [here](https://github.com/MSFTAuDX/SimpleBot/blob/master/SimpleIgniteBot/SimpleIgniteBot/LUIS/Partial_Luis_Cards.cs). 

<img alt="Session card" src="{{ site.baseurl }}/images/2017-04-07-nv/SessionActivityCard.PNG" width="320">
</img>

If the LUIS system does not match an intent (we have over 20 intents in the Ignite Bot system) then it will call the ```LuisDialog``` empty intent method which forms the mechanism for falling through to the next level - Qna Maker. 

```csharp
[LuisIntent("")]
public async Task NoIntent(IDialogContext context, LuisResult result)
{
```

### Setting up Cognitive Services Accounts ###

Most of the Cognitive Services require accounts to be created within an Azure subscription for billing. Log in to your Azure Portal and click the ">" at the bottom left and search for "cognitive". Click the little start logo next to the right hand side of the search area and to create a shortcut for later. 

<img alt="Cognitive Services in the Portal" src="{{ site.baseurl }}/images/2017-04-07-nv/CognitivePortal.PNG" width="320">
</img>

Within this are you can create keys for the various cognitive services such as LUIS and Bing Translator. 

For now you'll just need a new LUIS key Once you've created a key for LUIS

<img alt="Create a new Cognitive Service" src="{{ site.baseurl }}/images/2017-04-07-nv/CognitivePortalCreate.PNG" width="320">
</img>

Once your Cognitive Services account is created, select it and select "Keys". Copy your key and add it as a key to your Luis account from the publish tab. 

<img alt="Get your LUIS Keys" src="{{ site.baseurl }}/images/2017-04-07-nv/LuisKeys.PNG" width="320">
</img>

### Level 3: QnA Maker ###

During the development of the bot we asked the event organisers to produce a list of commonly asked questions and answers and populate them in to a spreadsheet. They went away and produced a list of nearly 200 question and answer pairs covering a variety of subjects. 

We then uploaded the Q&A pairs in to the [QnA Maker](http://qnamaker.ai/) intelligent service. 

<img alt="LUIS Json Output Example" src="{{ site.baseurl }}/images/2017-04-07-nv/QnaMakerPortal.PNG" width="720">
</img>

QnA maker takes your question and answer pairs and processes them using machine learning so that you can ask questions in a range of ways to find the correct results - similar to LUIS, but instead of intents you get answers. 

If LUIS fails to find an intent, then we send the query straight off to QnAMaker. The result is cached in Redis for a while to increase performance on common questions. 

QnA maker is again a straight up http endpoint that can be easily called. Code for how we make this call can be found [here](https://github.com/MSFTAuDX/SimpleBot/tree/master/SimpleIgniteBot/SimpleIgniteBot/QnaMaker). 

The magic happens in the "NoIntent" method which the BotBuilder base code will call if LUIS could not match any intents. 

```csharp
// Will run when no intent is triggered

[LuisIntent("")]
public async Task NoIntent(IDialogContext context, LuisResult result)
{
    try
    {   
        var props = new Dictionary<string, string> { { "Question", result?.Query } };

        var sentReply = await QueryQnaMakerAsync(context, result);
```

We parse out the original query and pass it through the the ```QueryMaker``` to build and send our query. The full source of QueryMaker can be found [here](https://gist.github.com/jakkaj/432796a53182d0c970ccf44d4ee87be6).

The gist of the query maker is that you need create a body and post it up to the QnA maker RESTful endpoint passing in the QnA Maker key with the ```Ocp-Apim-Subscription-Key``` header. 

```csharp
 var postBody = $"{{\"question\": \"{question}\", \"kbId\": \"{KbId}\"}}";

//Send the POST request
using (var client = new WebClient())
{
    client.Headers.Add("Content-Type", "application/json");
    client.Headers.Add("Ocp-Apim-Subscription-Key", this.SubscriptionKey);

    try
    {
        responseString = await client.UploadStringTaskAsync(uri, postBody);

```
([Full Listing](https://gist.github.com/jakkaj/432796a53182d0c970ccf44d4ee87be6))

If that call returns a result we send it straight back to the user as plain text. 

If QnA maker doesn't return any results then we pass through to the next level - Azure Search. 

### Level 4: Azure Search ###

Azure Search is a powerful and easy to use search system which we use to provide full text "fuzzy" searching across the full session catalog in the hope that when LUIS and QnA maker miss - the system will return something meaningful for the user. 

<img alt="Azure Search in the Portal" src="{{ site.baseurl }}/images/2017-04-07-nv/azuresearchportal.PNG" width="720">
</img>

First step is to set up a new Azure Search instance in the portal. Make sure it's hosted in the same datacenter as the rest of your system. Once the Search instance is ready, all you need from here is the instance url (from the overview screen) and key (keys section). 

The Azure Search catalog is updated by the timer based WebJob which has the ```Microsoft.Azure.Search``` Nuget package installed. 

When the bot system is initially set up a search catalog must be created. 

```csharp
private async Task CreateSearchIndexAsync()
{
    var definition = new Index()
    {
        Name = Constants.Search.SearchIndexName,
        Fields = FieldBuilder.BuildForType<SessionFacade>()
        
    };

    await _searchServiceClient.Indexes.CreateAsync(definition);
}

```
The ```SessionFacade``` entity is a flattened out version of the Ignite Session object (i.e. simpler, non-complex POCO type with only the fields that need to be indexed). 

```csharp
[Key]
[IsFilterable]
public string SessionId { get; set; }
[IsFilterable]        
public DateTimeOffset LastIndexed { get; set; }
[IsFilterable, IsSortable]
public string Name { get; set; }
[IsFilterable, IsFacetable, IsSortable]
public DateTimeOffset? StartDateTime { get; set; }
...
```
([Full Listing](https://gist.github.com/jakkaj/b42bb530d4152544a33bb4735a98f03f))

At regular intervals the Azure Web Job (below) will fire up and sync data from the Ignite Session Catalog backend system. After refreshing the session catalog from the Ignite Back end system, the search index is updated with any new content. This ensures the index is fresh and contains any changes and additions. 

For more information on building apps that use Azure Search see this [example](https://docs.microsoft.com/en-us/azure/search/search-howto-dotnet-sdk).


### Level 5: Bing Translator ###

In this scenario none of the prior levels have turned up any results. If (1)basic checks, (2)LUIS, (3)QnA Maker and (4)Azure Search all miss we look to verify which language the user is communicating in. 

For this we use the [Bing Translator Cognitive Service](https://www.microsoft.com/cognitive-services/en-us/translator-api).

1. Check language
2. If the detected language is not English, ask the user (in the detected language) if they would like the bot the translate for them. 
3. If the user responds in the affirmative, store a session state variable that can be checked later.
4. On the next message from the user, check session state for variable and if it's present start translating in and out automatically!

To service this scenario we built the [BotTranslator](https://github.com/MSFTAuDX/BotTranslator). This is a system that bolts on around your bot code and allows translation without each developer having to translate every action manually - i.e. it's transparent and super simple to use. The framework will handle the translation for you. 

Using the bot translator is simple, but there are some steps to go through to get it set up. 

Before you can begin you must set up a new Cognitive Services account in Azure much like the LUIS account. This time create a "Translator Text API" account type and collect the key. 

Firstly, add the [Simple Bot Translator Nuget Package](https://www.nuget.org/packages/SimpleBotTranslator/).

The next step is to edit your Web.Config file and add in the service model configuration. 

Add the following code just before the last ```</configuration>``` element. This is required because the Bing Translator uses an older style SOAP Xml end point. 

```xml
<system.serviceModel>
    <bindings>
        <basicHttpBinding>
        <binding name="BasicHttpBinding_LanguageService" />
        </basicHttpBinding>
    </bindings>
    <client>
        <endpoint address="http://api.microsofttranslator.com/V2/soap.svc" binding="basicHttpBinding" bindingConfiguration="BasicHttpBinding_LanguageService" contract="MicrosoftTranslator.LanguageService" name="BasicHttpBinding_LanguageService" />
    </client>
</system.serviceModel>
```

The next step is to ask the user if they would to start translating. 

This is done in the NoIntent method for when LUIS calls through after checks for QnA maker and Azure Search. 

```csharp
 var sentReply = await QueryQnaMakerAsync(context, result);

if (!sentReply)
{
    sentReply = await QueryAzureSearchAsync(context, result);

    if (!sentReply)
    {
        if (TranslatorService.Instance.GetLanguage(context) != "en")
        {
            var checkLanguage = await TranslatorService.Instance.Detect(result.Query);
            if (checkLanguage != "en")
            {
                context.UserData.SetValue("checkLanguage", checkLanguage);

                EditablePromptDialog.Choice(context,
                        LanuageSelectionChoicesAsync,
                            new List<string> { "Yes", "No" },
                            await TranslatorService.Instance.Translate("You are not speaking English! Would you like me to translate for you?", "en", checkLanguage),
                            await TranslatorService.Instance.Translate("I didn't understand that. Please choose one of the options", "en", checkLanguage),
                        2);

                return;}
        }

        await context.PostAsync("I'm sorry. I didn't understand you.");
        context.Wait(MessageReceived);
```
([Full Listing](https://github.com/MSFTAuDX/BotTranslator/blob/master/Samples/PizzaBot/PizzaOrderDialog.cs))


The next step is to start by intercepting the incoming bot call and perform some checks to see if the user has opted in to translation - which is all handled by the BotTranslator for you. Add the following line to initialise the translator in ```MessagesController.cs```.

```csharp
[ResponseType(typeof(void))]
public virtual async Task<HttpResponseMessage> Post([FromBody] Activity activity)
{
    TranslatorService.Instance.SetKey("[Your key here]");
```

Once that is done you can begin to use the translator. 

Before the user's message is sent off to LUIS for processing you must ask the translation service to intercept and translate the call. 

```csharp
await TranslatorService.Instance.TranslateIn(activity, true);
```
([Full Listing](https://github.com/MSFTAuDX/BotTranslator/blob/master/Samples/PizzaBot/Controllers/MessagesController.cs))

Under the covers that is checking the user state to see if translation has been enabled. 

### Your Code - how to translate ###

Once translation has been set up it's easy to start translate in and out of your own custom bot code. 

Firstly - any time you write plain text back to the user with ```context.PostAsync``` you don't need to do anything - the BotTranslator framework code will translate for you. This includes Azure Search and QnA maker code. 

Where you do need to make considerations is around dialogs - you'll have to use the special ```EditablePromptDialog``` to have translatable dialogs in the system. 

```csharp
EditablePromptDialog.Choice(context,
    PizzaFormComplete,
    topicChoices,
    "Which toppings?",
    "I didn't understand that. Please choose one of the toppings",
    2);

...

private async Task PizzaFormComplete(IDialogContext context, IAwaitable<string> result)
{

```

This will allow your system to create translatable dialogs without you having to manage the actual translation code throughout the app (which can get large and un-manageable over time.)

Remember to check out the [BotTranslator GitHub repo](https://github.com/MSFTAuDX/BotTranslator) for full instructions. 


### Bot Connector ###
The last piece of the puzzle was connecting up to the [Bot Connector](https://dev.botframework.com/) and exposing the actual bot via Skype and Teams. The premise is you create an account on [https://dev.botframework.com/](https://dev.botframework.com/). 

For full documentation on bot publishing see [here](https://docs.botframework.com/en-us/directory/publishing/). 

### Application Insights ###
We relied heavily on [Application Insights](https://azure.microsoft.com/en-au/services/application-insights/) telemetry during the event to help us adjust the system for real world usage. Systems like LUIS and QnA maker have UI built in to help you take real world data and re-apply it to he ML models, as mentioned in the last section. 

These however only caught a small glimpse of how the system was being used. With Application Insights we were able to break down and see in near real-time how the app was being used from server loads, response times right trough to the questions being asked. 

This real world view is critical for production applications. 

Application Insights is very easy to get going - it can be added to a web site with one click in Visual Studio, or you can create an Application Insights instance from the Azure Portal. 

We opted to use a pre-build wrapper class from the nuget package ```Xamling-Azure``` as it works nicely with our dependency injection pattern ([LogService.cs](https://github.com/jakkaj/Xamling-Azure/blob/master/Xamling.Azure/Logger/LogService.cs)).

We injected an instance of that class in to most our bot web site classes. We then tracked everything from exceptions to what the user was asking of the bot. 

```csharp
catch (Exception e)
{
    _tc.TrackException(e);
```
or
```csharp
string intent = result.Intents[0].Intent;
_tc.TrackEvent(intent);
```

### Backgound Sync with Azure Webjobs ###

The Ignite Conference "source of truth" is in a back end system called Arlo. A synchonisation process based in an App Service Webjob was run every few hours to ensure data in the bot system was kept up to speed. 

We decided it was best to sync the data to a local cache for speed - plus we needed to iterate through all the sessions to create the Azure Search index anyway. 

Creating a Webjob is easy - from the Website.csproj, right click and select Add -> New AzureWeb Job Project. Select manual trigger. Next we will use publishing templates to set up a scheduler call to this WebJob.

In Properties/webjob-publish-settings.json file add the following to create the scheduler task during publication (replace the name with your own). 

```json
{
  "$schema": "http://schemastore.org/schemas/json/webjob-publish-settings.json",
  "webJobName": "SyncJob",
  "startTime": "2015-12-16T00:00:00+10:00",
  "endTime": "2020-12-16T00:00:00+10:00",
  "jobRecurrenceFrequency": "Hour",
  "interval": 1,
  "runMode": "Scheduled"
}
```
Now the webjob will run every hour. 

When the sync job runs, it resolves the services it needs using the same dependency resolution pattern as the rest of the app.  

```csharp
[NoAutomaticTrigger]
public static async Task ManualTrigger(TextWriter log)
{
    var g = new ProjectGlue();
    var container = g.Init();

    var syncService = container.Resolve<ISyncService>();
    
    log.WriteLine("Webjob beginning Arlo sync");

    var result = await syncService.SyncAll();

    log.WriteLine($"Job result: {result}");

    var searchService = container.Resolve<IArloSdkSearchIndexService>();

    await searchService.InitAsync();
    await searchService.IndexAsync();

    await Task.Delay(10000);
}
```

Each of these services are noted earlier on in this document. 

This sync process stages the data in an Azure Redis Cache - keeping the data nice and local to the bot for fast access. 

### Authentication ###

One of the more complex parts of the system is Authentication. 

If the user tries for perform an operation that requires an authenticated session (such as add a new session) then the system will ask them to log in and provide a link. 

This links goes to the login bounce page on the bot website which will redirect the user to the Ignite Website oAuth page for login on the original site. This site returns a JWT (Json Web Token) that we can cryptographically check for authenticity before extracting claims about the user - such as their Arlo Id. 

A key component of this redirection process is the resumption cookie. 

```csharp
var encodedResumptionCookie = UrlToken.Encode(_resumptionCookie);

var authUri = GetLoginUrl(encodedResumptionCookie);

```
([Full Listing](https://gist.github.com/jakkaj/9b815903e05bb5500de01b0cde8c3678))
This cookie which can be passed around on the URL query string allows us to resume the session later (after login) and set the user's details. 

When the process bounces back to the ```AutenticationController```,  the system uses the resumption cookie to rehydrate the session and process the login (from the "rc" query string variable)

```csharp

var authorizationState = _webServerClient.ProcessUserAuthorization(this.Request);
if (authorizationState != null)
{
    var accessToken = authorizationState.AccessToken;

    var rc = queryString["rc"];

    var loginService = ContainerHost.Container.Resolve<ILoginHandlerService>();
    var result = await loginService.HandleLoginReturned(rc, accessToken);
```

([Full Listing](https://gist.github.com/jakkaj/7cdfa3a44bda4e7b2156c83d18e17fc8))

```loginService.HandleLoginReturned(rc, accessToken);``` is in ([this gist](https://gist.github.com/jakkaj/9b815903e05bb5500de01b0cde8c3678)). It contains the flow on how to extract the JWT and update the bot session state to reflect the fact the user is logged in and who they are. 



### Arlo SDK and Data Access Pattern ###

Arlo uses a RESTful API - meaning data is retrieved by correctly crafting URI's to get the data. When a new resource is needed from Arlo, the URI is constructed, then Redis is checked to see if it contains the data for that URI. If not, a call is made to Arlo to get that data and the data is saved in Redis. 

We do this but using a fall through pattern implemented in ```CachingService.cs``` 

```IRedisEntityCache``` is injected in to ```CachingService``` allowing it to operate with Redis to check URI's before they are sent off to the Arlo back end system. 

```csharp
 public CachingService(IRedisEntityCache entityCache)
{
    _entityCache = entityCache;
}

public async Task SetEntity<T>(string key, T entity, TimeSpan? ts = null)
    where T:class, new()
{
    await _entityCache.SetEntity(key, entity, ts ?? Constants.Cache.DefaultTimespan);
}

public async Task<T> GetEntity<T>(string key)
        where T : class, new()
{
    return await _entityCache.GetEntity<T>(key);
}
```

([Full Listing](https://github.com/MSFTAuDX/ArloSdk/blob/c5bf5ce64c3bf53508b1a26e80bc34f05deef2fd/source/Arlo.SDK/Services/System/CachingService.cs))

([RedisEntityCache Full Listing](https://github.com/jakkaj/Xamling-Azure/blob/01ce43a3a7dfdabb1d5071a178af07e368d087bf/Xamling.Azure/Redis/EntityCaches/RedisEntityCache.cs))

Now when the systems asks for something it can perform the fall through requests.

 ```csharp
var cacheResult = await GetEntity<List<T>>(key);

if (cacheResult != null && !forceRefresh)
{
    return cacheResult;
}
 ```

```CachingService.cs``` is an abstracted base class for other services - it's methods are generic and extensible. Whilst this may look complex - it in fact hides away a lot of complexities from the calling code. 

For example, all that complexity discussed above can make a simple call to get Event Sessions (i.e. Ignite sessions) like this:

```csharp
public async Task<List<ArloSession>> GetEventSessions(List<Link> sessionLinks, bool forceRefresh)
{
    if (sessionLinks == null)
    {
        return new List<ArloSession>();
    }

    var resultTasks = new List<Task<ArloSession>>();

    foreach (var item in sessionLinks.Where(_ => _.Rel == Constants.Rel.EventSession))
    {
        var i2 = item;
        var t = TaskThrottler.Get("GetSessonLinks", 30).Throttle(() => GetEventSession(i2.Href, forceRefresh));
        resultTasks.Add(t);
    }

    var resultFromTasks = await Task.WhenAll(resultTasks);

    var listResult = resultFromTasks.ToList();

    return listResult;
}
```

No need for Redis references, knowledge of caching mechanisms or internal esoteric knowledge of the Arlo system itself. Just send in some links to get the result. 



## Learnings and Cautions ##

### Azure Region Selection ###

During testing after deployment to Azure our bot was responding slowly. We'd deployed it to the Australia East data center in Sydney. 

We had a theory that it was slow due to transmission delays to Skype and the Cognitive Services which are hosted in North America. We set up a range of Virtual Machines in data-centers around the US to test the speed to LUIS and Bing Translator and through this method discovered that West US data-center was best for our bot. 

Surely enough, once we moved out app to this DC and re-set up the bot connector the speed of the bot increase many fold - taking us from comments that "it's a bit slow isn't it" to "wow that's awesome, almost instant for some queries". 

### Publishing the Bot ###

During development we had the bot unpublished to ensure the public would not see it. During the Keynote of the conference we announced the bot and people started to install it - but it was left un-published. This meant that only a certain number of people could install the bot before it stopped accepting friend requests. We were able to have the bot published quickly, but the learning is that bots must be published before they are sent in to a production environment. 

### Machine Language / Intelligent systems take time to be awesome ###
 
Machine Learning systems get better as they are exposed to more data - straight up, the more data you can provide them the better they are. Importantly with LUIS there are two distinct types of data - the first being the configuration data that is used to train the model. The second is real world usage and refinement data that you gather and apply over time as the bot is in production and use to re-train the models for more accuracy. The second data only comes after time.

LUIS and QnA maker have [systems built(https://www.microsoft.com/cognitive-services/en-us/LUIS-api/documentation/Label-Suggested-Utterances)] in to help facilitate this and I strongly recommend you use them.  

It's very important to set expectations with stakeholders that LUIS (like any ML based system) will work well on day one, but it will only really hit its stride after time - Perhaps months even.  

### Loosely coupled design ###

Bots start off very simple (the most basic of Node.js bots can be created with just a few lines of code!) but like other app types - over time they will become large applications. It's important that good code design is used from the beginning. 

We worked very hard during the construction of the bot to make sure we were using up to date design patterns specially around Dependency Injection and having loosely coupled dependencies. 

This allowed us to later on extract the BotTranslator out in to it's own project for community use. It will also allow NV Interactive to extract out parts for re-use with other similar events. 


## Conclusion ##

The bot was a great success, serving nearly 5000 questions during the event without any downtime. Attendees were positive to the bot and the various bot and conversational platform related sessions were well attended. 

The bot successfully introduced and demonstrated a range of Cognitive Services in a real world situation to attendees. For many the demos during the Keynote was the first introduction to LUIS and QnA Maker. Being backed by real world scenarios gave a better view in to why the services are important. 

The total time for the build was approximately two weeks for two developers - with a large chunk of that time being taken up by the back end service integration (there was no SDK - so we had to build one). With timelines like this it would be easy to make the decision to invest in an event bot again for the next event. 



## Additional resources ##
In this section, include a list of links to resources that complement your story, including (but not limited to) the following:

- [Bot Framework](https://dev.botframework.com/)
- [Simple Ignite Bot sample code](https://github.com/MSFTAuDX/SimpleBot)
- [Bot Translator Source and Samples](https://github.com/MSFTAuDX/BotTranslator)
- [LUIS](https://www.microsoft.com/cognitive-services/en-us/language-understanding-intelligent-service-luis)
    - [LUIS Doco](https://www.luis.ai/home/help)
    - [LUIS Active Learning](https://www.microsoft.com/cognitive-services/en-us/LUIS-api/documentation/Label-Suggested-Utterances)
    - [Sample LUIS Project for Ignite Australia 2017](https://gist.github.com/jakkaj/02f8f7d2152c0e9403342be608e05754)
- [QnA Maker](https://qnamaker.ai/)
- [Azure Search .NET SDK](https://docs.microsoft.com/en-us/azure/search/search-howto-dotnet-sdk) ([Node.js SDK](https://docs.microsoft.com/en-us/azure/search/search-get-started-nodejs))
    - [Deep Azure Search .NET Example](https://docs.microsoft.com/en-us/azure/search/search-howto-dotnet-sdk)
- [Bing Translator](https://www.microsoft.com/cognitive-services/en-us/translator-api)
- [Application Insights .NET](https://docs.microsoft.com/en-us/azure/application-insights/app-insights-asp-net)
    - [Application Insights Node.js](https://docs.microsoft.com/en-us/azure/application-insights/app-insights-nodejs)
- [Arlo SDK](https://github.com/MSFTAuDX/ArloSdk)


#### Videos ####

[Ignite Bot Keynote Demo](https://channel9.msdn.com/Events/Ignite/Australia-2017/Keynote1#time=1h01m00s)

[Making of the Ignite Bot breakout session](https://channel9.msdn.com/Events/Ignite/Australia-2017/CLD232)

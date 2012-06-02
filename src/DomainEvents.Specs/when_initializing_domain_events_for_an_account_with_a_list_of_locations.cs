using System.Collections.Generic;
using System.Linq;
using DomainEvents.Testing;
using Machine.Specifications;

namespace DomainEvents.Specs
{
    public class when_initializing_domain_events_for_an_account_with_a_list_of_locations
    {
        static DomainEventInitializer _initializer;
        static Account _account;
        static TestDomainEventDispatcher _testDomainEventDispatcher;

        Establish context = () =>
            {
                _testDomainEventDispatcher = new TestDomainEventDispatcher();
                _initializer = new DomainEventInitializer(_testDomainEventDispatcher);
                var locationInList1 = new Location();
                var locationInList2 = new Location();
                _account = new Account
                    {
                        OldLocations = new List<Location>
                            {
                                locationInList1,
                                locationInList2,
                            }
                    };
            };

        Because of = () =>
            {
                _initializer.Initialize(_account);
                _account.OldLocations.First().ChangeLocation("changing list location #1");
                _account.OldLocations.Last().ChangeLocation("changing list location #2");
            };

        It should_have_dispatched_an_event_on_a_collection_member =
            () =>
            _testDomainEventDispatcher.WithEventsDispatched<LocationChanged>()
                .ShouldContain(x => x.NewLocation == "changing list location #1");

        It should_have_dispatched_an_event_on_another_collection_member =
            () =>
            _testDomainEventDispatcher.WithEventsDispatched<LocationChanged>()
                .ShouldContain(x => x.NewLocation == "changing list location #2");


    }
}
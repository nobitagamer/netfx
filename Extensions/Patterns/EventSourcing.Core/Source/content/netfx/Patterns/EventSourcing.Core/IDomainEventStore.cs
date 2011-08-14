﻿#region BSD License
/* 
Copyright (c) 2010, NETFx
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

* Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.

* Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

* Neither the name of Clarius Consulting nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

/// <summary>
/// Interface implemented by domain event stores.
/// </summary>
/// <typeparam name="TAggregateId">The type of identifier used by the aggregate roots in the domain.</typeparam>
/// <typeparam name="TBaseEvent">The base type or interface implemented by events in the domain.</typeparam>
/// <nuget id="netfx-Patterns.EventSourcing.Core"/>
partial interface IDomainEventStore<TAggregateId, TBaseEvent>
	where TAggregateId : IComparable
{
	/// <summary>
	/// Gets or sets the function that converts a <see cref="Type"/> to 
	/// its string representation in the store. Used to calculate the 
	/// values of <see cref="IStoredEvent{TAggregateId}.AggregateType"/> and 
	/// <see cref="IStoredEvent{TAggregateId}.EventType"/>.
	/// </summary>
	Func<Type, string> TypeNameConverter { get; set; }

	/// <summary>
	/// Notifies the store that the given event raised by the given sender 
	/// should be persisted when <see cref="Commit"/> is called.
	/// </summary>
	/// <param name="sender">The sender of the event.</param>
	/// <param name="args">The instance containing the event data.</param>
	void Persist(AggregateRoot<TAggregateId, TBaseEvent> sender, TBaseEvent args);

	/// <summary>
	/// Queries the event store for events that match the given criteria.
	/// </summary>
	/// <remarks>
	/// This is the only low-level querying method that stores need to implement. 
	/// <para>
	/// As a facility for stores that persist events in an <see cref="IQueryable{T}"/> 
	/// queryable object, the <see cref="StoredEventCriteria{TAggregateId}"/> object 
	/// can be converted to an expression using the <see cref="StoredEventCriteriaExtensions.ToExpression{TAggregateId}"/> 
	/// extension method, making the query implementation trivial in that case.
	/// </para>
	/// <para>
	/// The more user-friendly querying API in <see cref="IDomainEventQuery{TAggregateId, TBaseEvent}"/> 
	/// leverages this method internally and therefore can be used by any 
	/// event store implementation. It's accessible by executing the 
	/// <see cref="DomainEventQueryExtensions.Query{TAggregateId, TBaseEvent}"/> over the 
	/// store instance.
	/// </para>
	/// </remarks>
	IEnumerable<TBaseEvent> Query(StoredEventCriteria<TAggregateId> criteria);

	/// <summary>
	/// Persists all <see cref="Persist"/>s performed so far, effectively commiting 
	/// the changes to the underlying store in a unit-of-work style.
	/// </summary>
	void Commit();
}
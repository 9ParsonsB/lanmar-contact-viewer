<script lang="ts">
    import type Contact from "../../types/contact";
    import api from "$lib/api.svelte";
    import {Circle} from "svelte-loading-spinners";
    import {onMount} from "svelte";
    let contactPromise: Promise<Contact> = $state(new Promise(() => {}))
    let slug = $state<undefined | string>("");
    onMount(() => {
        const urlParams = new URLSearchParams(window.location.search);
        slug = urlParams.get("id") ?? slug;
        if (slug === "add" || slug === "" || !slug)
        {
            contactPromise = Promise.resolve({firstName: "", lastName: "", email: "", company: "", phone: "", id:"", lastUpdated: ""})
            return;
        }
        contactPromise = api.get(slug);
        contactPromise.then((contact) => {
            firstName = contact.firstName;
            lastName = contact.lastName;
            company = contact.company;
            email = contact.email;
            phone = contact.phone;
            lastUpdated = new Date(contact.lastUpdated);
        });
    })
    
    let firstName = $state("");
    let lastName = $state(""); 
    let company = $state("");
    let email = $state("");
    let phone = $state("");
    let id = $derived(() => slug === "add" || !slug ? undefined : slug);
    let lastUpdated = $state<Date | undefined>(undefined)
    
    let savePromise: Promise<void> | undefined = $state();
    
    const save = (): void => {
        let contact = {firstName: firstName, lastName: lastName, company: company, email: email, phone: phone}
        if (id && lastUpdated)
        {
            savePromise = api.update({...contact, id: id()!, lastUpdated: lastUpdated});
        }
        savePromise = api.create(contact);
    }
</script>

<button class="py-2 bg-red-300 hover:bg-red-400 rounded-lg px-4 m-2"><a href="/">back</a></button>

<div class="max-w-lg mx-auto p-6 bg-white shadow-lg rounded-lg">
    <h2 class="text-2xl font-bold mb-4">Add Contact</h2>

    {#if savePromise}
        {#await savePromise}
            Saving... <Circle />
        {:then saved}
            Saved! 
        {:catch error}
            Error! {error}
        {/await}
    {:else }
        {#await contactPromise}
            Loading... <Circle />
        {:then _}
            <div class="space-y-4">
                <input
                        type="text"
                        bind:value={firstName}
                        placeholder="First Name"
                        class="w-full px-4 py-2 border rounded-lg focus:ring focus:ring-blue-300"
                />
                <input
                        type="text"
                        bind:value={lastName}
                        placeholder="Last Name"
                        class="w-full px-4 py-2 border rounded-lg focus:ring focus:ring-blue-300"
                />
                <input
                        type="text"
                        bind:value={company}
                        placeholder="Company"
                        class="w-full px-4 py-2 border rounded-lg focus:ring focus:ring-blue-300"
                />
                <input
                        type="text"
                        bind:value={phone}
                        placeholder="Phone"
                        class="w-full px-4 py-2 border rounded-md"
                />
                <input
                        type="email"
                        bind:value={email}
                        placeholder="Email"
                        class="w-full px-4 py-2 border rounded-lg focus:ring focus:ring-blue-300"
                />
                <button
                        onclick={() => save()}
                        class="w-full bg-blue-500 text-white py-2 rounded-lg hover:bg-blue-600"
                >
                    {!!id() ? "Update" :"Create"} Contact
                </button>
            </div>
        {:catch error}
            Failed to loading contact. {error}
        {/await}
    {/if}
</div>
<script lang="ts">
    import api from "$lib/api.svelte";
    import {fade, slide} from "svelte/transition"
    import {onMount} from "svelte";
    import type Contact from "../types/contact";
    import {BarLoader} from "svelte-loading-spinners";
    
    let contactsPromise = $state<Promise<Contact[]>>(new Promise(()=>{}));
    onMount(() => {
        contactsPromise = api.getAll(query);
    })
    $effect(() => {
        contactsPromise = api.getAll(query);
    })
    let query = $state("")
    
    const openContact = (c: Contact) => {
        window.location.href = "/contact?id=" + c.id;
    }
</script>

<h1 class="text-3xl font-bold text-center text-gray-800 mt-6">
    Welcome to Contacts
</h1>

<section class="flex w-full justify-center">
<button class="mt-4 bg-blue-500 text-white px-4 py-2 rounded-lg hover:bg-blue-600 transition">
    <a href="contact?id=add" class="block w-full h-full">Create a new contact</a>
</button>
</section>

<div class="mt-6 flex justify-center p-5">
    <input
            in:fade
            type="text"
            placeholder="Search"
            bind:value={query}
            class="w-full max-w-md px-4 py-2 border rounded-lg focus:ring focus:ring-blue-300"
    />
</div>
<ul>
    {#await contactsPromise}
        <BarLoader/>
    {:then contacts}
        <div class="overflow-x-auto flex justify-center">
            <table class="w-5/6 text-center content-center items-center border-collapse border border-gray-300">
                <thead>
                <tr class="bg-gray-100">
                    <th class="border p-2 text-left">First Name</th>
                    <th class="border p-2 text-left">Last Name</th>
                    <th class="border p-2 text-left">Company</th>
                    <th class="border p-2 text-left">Phone</th>
                    <th class="border p-2 text-left">Email</th>
                </tr>
                </thead>
                <tbody>
                {#each contacts as contact (contact.id)}
                    <tr class="hover:bg-gray-50 cursor-pointer" onclick={()=>openContact(contact)} in:slide>
                        <td class="border p-2">{contact.firstName}</td>
                        <td class="border p-2">{contact.lastName}</td>
                        <td class="border p-2">{contact.company}</td>
                        <td class="border p-2">{contact.phone}</td>
                        <td class="border p-2">{contact.email}</td>
                    </tr>
                {/each}
                </tbody>
            </table>
        </div>
    {:catch error}
        An error occured {error}
    {/await}
</ul>
<script lang="ts">
    import Api from "$lib/api";
    import {fade, slide} from "svelte/transition"

    let api = new Api();
    let contactsPromise = $derived(() => 
        api.getAll(query)
    );
    let query = $state("")
</script>

<h1>Welcome to Contacts</h1>

<div class="search">
    <input
            in:fade
            type="text"
            placeholder="Search"
            bind:value={query}
    />
</div>

<div in:slide>
    <span>
      Contact Name
    </span>
</div>
<ul>
    {#await contactsPromise()}
        loading...
    {:then contacts}
        {#each contacts as contact (contact.id)}
            <li>
                {contact.firstName}
            </li>
        {/each}
    {:catch error}
        An error occured {error}
    {/await}
</ul>
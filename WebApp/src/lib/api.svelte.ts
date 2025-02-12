import type Contact from "../types/contact";

export class ApiSvelte {
    private apiUrl = "/api";

    /**
     * Get a specific contact by Id
     * @param id the Id to get the contact by, should be a guid
     */
    public get = async (id: string) => {
        return await (await fetch(`${this.apiUrl}/${id}`)).json() as Contact;
    };

    /**
     * Get All contacts with a free text query
     * @param query
     */
    getAll = async (query: string) =>
        (await (await fetch(`${this.apiUrl}?${new URLSearchParams({"query": query})}`)).json()) as Contact[];

    /**
     * Create a Contact
     * @param data Contact to create
     * @returns Id of created contact
     */
    create = async (data: Omit<Omit<Contact, "id">, "lastUpdated">) => {
        await (await fetch(`${this.apiUrl}`, {
            method: "POST", body: JSON.stringify(data), headers: {
                "Content-Type": "application/json",
            }
        }));
    }

    /**
     * Update a specified contact, by their Id
     * @param contact contact to update
     */
    update = async (contact: Contact) => {
        await (await fetch(`${this.apiUrl}/${contact.id}`, {
            method: "PUT", headers: {
                "Content-Type": "application/json",
            }
        })).json()
    };

}

const api = $state<ApiSvelte>(new ApiSvelte());

export default api;